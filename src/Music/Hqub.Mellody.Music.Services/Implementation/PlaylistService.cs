using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Hqub.Mellody.Music.Commands;
using Hqub.Mellody.Music.Configure;
using Hqub.Mellody.Music.Services.Exceptions;
using Hqub.Mellody.Music.Services.Interfaces;
using Hqub.Mellody.Music.Store;
using Hqub.Mellody.Poco;
using Playlist = Hqub.Mellody.Music.Store.Models.Playlist;
using Track = Hqub.Mellody.Music.Store.Models.Track;

namespace Hqub.Mellody.Music.Services
{
    public class PlaylistService : IPlaylistService
    {

        #region Fields

        private readonly ILogService _logService;
        private readonly PlaylistConfigureSection _configure;
        private readonly ICacheService _cacheService;
        private readonly IEchonestService _echonestService;
        private readonly CommandFactory _mellodyTranslator;
        private readonly Dictionary<Type, Func<List<Entity>, Task<List<Track>>>> _mappingCommand;

        #endregion

        #region .ctor

        public PlaylistService(ILogService logService,
            IConfigurationService configurationService,
            ICacheService cacheService,
            IEchonestService echonestService)
        {
            _logService = logService;
            _configure = configurationService.GetPlaylistConfig();
            _cacheService = cacheService;
            _echonestService = echonestService;

            _mappingCommand = new Dictionary<Type, Func<List<Entity>, Task<List<Track>>>>
            {
                {
                    typeof (TrackCommand), GetTracks
                },

                {
                    typeof (AlbumCommand), GetAlbums
                },

                {
                    typeof (ArtistCommand), GetArtists
                },

                {
                    typeof(GenreCommand), GetTracksByGenre
                }
            };

            _mellodyTranslator = new CommandFactory();
        }

        #endregion

        #region IPlaylistService

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

                var queryText = GetQueryText(query);

                var command = _mellodyTranslator.Create(queryText);
                if (command == null)
                    return playlist;

                playlist.Tracks = new Collection<Track>(await _mappingCommand[command.GetType()](command.Entities));
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
                return context.Tracks.Where(t => t.Playlist.Id == id).Select(t => new Track
                {
                    Artist = t.Artist,
                    Title = t.Title,
                    Duration = t.Duration,
                    Id = t.Id
                }).OrderBy(t => t.Position).ToList();
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Check type and transfom text query.
        /// </summary>
        /// <param name="query">User query</param>
        /// <returns>Transformed query</returns>
        private string GetQueryText(QueryEntity query)
        {
            switch (query.TypeQuery)
            {
                case TypeQuery.Album:
                    return string.Format("album \"{0}\"", query.Name.Trim());
                case TypeQuery.Artist:
                    return string.Format("group \"{0}\"", query.Name.Trim());
                case TypeQuery.Genre:
                    return string.Format("genre \"{0}\"", query.Name.Trim());
                default:
                    return query.Name;
            }
        }

        private async Task<List<Track>> GetTracks(IEnumerable<Entity> entities)
        {
            return entities.Select(t => new Track
            {
                Id = Guid.NewGuid(),
                Artist = t.Artist,
                Title = string.Format("{0} - {1}", t.Artist, t.Track)
            }).ToList();
        }

        private async Task<List<Track>> GetAlbums(List<Entity> entities)
        {
            var tracks = new List<Track>();

            foreach (var entity in entities)
            {
                try
                {
                    var album = await Helpers.MusicBrainzHelper.GetAlbumTracks(entity.Artist, entity.Album);

                    tracks.AddRange(album.Releases.Select(releaseInfo => new Track
                    {
                        Id = Guid.NewGuid(),
                        MbId = Guid.Parse(releaseInfo.Recording.Id),
                        Artist = album.Artist,
                        Title = album.Album,
                        Duration = releaseInfo.Recording.Length,
                        Position = releaseInfo.Position
                    }).ToList());
                }
                catch (Exception exception)
                {
                    LogException("GetAlbums", entities, exception);
                }
            }

            return tracks;
        }

        private async Task<List<Track>> GetArtists(List<Entity> entities)
        {
            var tracks = new List<Track>();

            foreach (var entity in entities)
            {
                try
                {
                    var artistTracksInfo =
                        await Helpers.MusicBrainzHelper.GetArtistTracks(entity.Artist, _configure.MaxTracks);

                    tracks.AddRange(artistTracksInfo.Recordings.Select(t => new Track
                    {
                        Id = Guid.NewGuid(),
                        MbId = Guid.Parse(t.Id),
                        Artist = artistTracksInfo.Artist,
                        Title = t.Title,
                        Duration = t.Length
                    }));
                }
                catch (Exception exception)
                {
                    LogException("GetArtists", entities, exception);
                }
            }

            return tracks;
        }

        private async Task<List<Track>> GetTracksByGenre(List<Entity> entities)
        {
            var tracks = new List<Track>();

            try
            {
              var echoTracks =  _echonestService.GetPlaylistByGenre(entities.Select(e => e.Genre).ToList(), 100);

                tracks.AddRange(echoTracks.Tracks.Select(t=> new Track
                {
                    Id = Guid.NewGuid(),
                    Artist = t.ArtistName,
                    Title = t.Title
                }));
            }
            catch (Exception exception)
            {
                LogException("GetTracksByGenre", entities, exception);
            }

            return tracks;
        }

        private void LogException(string methodName, List<Entity> entities, Exception exception)
        {
            var builder = new StringBuilder(string.Format("PlaylistService.{0}", methodName));
            foreach (var entity1 in entities)
                builder.AppendFormat("\tentity: {0}}\n", entity1.Artist);

            _logService.AddExceptionFull(builder.ToString(), exception);
        }


        #endregion
    }
}