using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Commands;
using Hqub.Mellody.Music.Configure;
using Hqub.Mellody.Music.Services.Interfaces;
using Hqub.Mellody.Music.Store;
using Hqub.Mellody.Poco;
using Lastfm.Services;
using Playlist = Hqub.Mellody.Music.Store.Models.Playlist;
using Track = Hqub.Mellody.Music.Store.Models.Track;

namespace Hqub.Mellody.Music.Services.Implementation
{
    public class LastfmPlaylistService : IPlaylistService
    {
        #region Fields

        private readonly ILogService _logService;
        private readonly PlaylistConfigureSection _configure;
        private readonly ICacheService _cacheService;
        private readonly ILastfmService _lastfmService;
        private readonly IEchonestService _echonestService;

        private readonly Dictionary<TypeQuery, Func<QueryEntity, Task<List<Track>>>> _mappingCommand;

        #endregion

        #region .ctor

        public LastfmPlaylistService(ILogService logService,
            IConfigurationService configurationService,
            ICacheService cacheService,
            ILastfmService lastfmService,
            IEchonestService echonestService)
        {
            _logService = logService;
            _configure = configurationService.GetPlaylistConfig();
            _cacheService = cacheService;
            _lastfmService = lastfmService;
            _echonestService = echonestService;

            _mappingCommand = new Dictionary<TypeQuery, Func<QueryEntity, Task<List<Track>>>>
            {
                {
                    TypeQuery.Artist, GetArtists
                },

                {
                    TypeQuery.Album, GetAlbums
                },

                {
                    TypeQuery.Genre, GetGenreTracks
                },

                {
                    TypeQuery.Track, GetTracks
                }
            };
        }

        #endregion

        public async Task<Playlist> Create(QueryEntity query)
        {
            var playlist = _cacheService.GetPlaylist(query);
            if (playlist != null)
                return playlist;

            using (var ctx = new MusicStoreDbContext())
            {
                var playlistId = Guid.NewGuid();
                playlist = new Playlist
                {
                    Id = playlistId,
                    Name = Helpers.PlaylistHelper.GetArtistName(query)
                };

                ctx.Playlists.Add(playlist);

                playlist.Tracks = new Collection<Track>(await _mappingCommand[query.TypeQuery](query));
                if (playlist.Tracks.Count == 0)
                    return null;

                _cacheService.AddPlaylist(query, playlist);

                ctx.SaveChanges();

                return playlist;
            }
        }

        public List<Track> Get(Guid id)
        {
            using (var context = new MusicStoreDbContext())
            {
                return context.Tracks.OrderBy(t => t.Position).ToList();
            }
        }

        #region Private Methods

        private async Task<List<Track>> GetArtists(QueryEntity entity)
        {
            var tracks = new List<Track>();

            try
            {
                var lastfmTracks = _lastfmService.GetArtistTracks(entity.MbId);

                tracks = lastfmTracks.Select(ConvertLastFmTrack).ToList();
            }
            catch (Exception exception)
            {
                LogException("GetArtists", entity, exception);
            }

            return tracks;
        }

        private async Task<List<Track>> GetAlbums(QueryEntity entity)
        {
            var tracks = new List<Track>();

            try
            {
                var albumTracks = _lastfmService.GetAlbumTracks(entity.MbId);

                tracks.AddRange(albumTracks.Select(ConvertLastFmTrack));
            }
            catch (Exception exception)
            {
                LogException("GetAlbums", entity, exception);
            }

            return tracks;
        }

        private async Task<List<Track>> GetGenreTracks(QueryEntity entity)
        {
            var tracks = new List<Track>();

            try
            {
                var echoTracks = _echonestService.GetPlaylistByGenre(new List<string>
                {
                    entity.Name
                }, 100);

                tracks.AddRange(echoTracks.Tracks.Select(t => new Track
                {
                    Id = Guid.NewGuid(),
                    Artist = t.ArtistName,
                    Title = t.Title
                }));
            }
            catch (Exception exception)
            {
                LogException("GetGenreTracks", entity, exception);
            }

            return tracks;
        }

        private async Task<List<Track>> GetTracks(QueryEntity entity)
        {
            throw new NotImplementedException();
        }

        private Track ConvertLastFmTrack(Lastfm.Services.Track track)
        {
            return new Track
            {
                Artist = track.Artist.Name,
                Title = track.Title,
                Id = Guid.NewGuid()

            };
        }

        private void LogException(string methodName, QueryEntity entity, Exception exception)
        {
            var builder = new StringBuilder(string.Format("PlaylistService.{0}", methodName));
            builder.AppendFormat("\tentity: {0}\n", entity.Name);
                
            _logService.AddExceptionFull(builder.ToString(), exception);
        }

        #endregion
    }
}
