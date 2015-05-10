using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Hqub.Mellody.Music.Commands;
using Hqub.Mellody.Music.Services.Exceptions;
using Hqub.Mellody.Music.Store;
using Hqub.Mellody.Poco;
using Playlist = Hqub.Mellody.Music.Store.Models.Playlist;
using Track = Hqub.Mellody.Music.Store.Models.Track;

namespace Hqub.Mellody.Music.Services
{
    public class PlaylistService : IPlaylistService
    {

        #region Fields

        private readonly ICacheService _cacheService;
        private readonly CommandFactory _mellodyTranslator;
        private readonly Dictionary<Type, Func<List<Entity>, Task<List<Track>>>> _mappingCommand;

        #endregion

        #region .ctor

        public PlaylistService(ICacheService cacheService)
        {
            _cacheService = cacheService;
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
                }).ToList();
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

                    tracks.AddRange(album.Tracks.Select(t => new Track
                    {
                        Id = Guid.Parse(t.Id),
                        Artist = entity.Artist,
                        Title = t.Title,
                        Duration = t.Length
                    }).ToList());
                }
                catch
                {
                    continue;
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
                    var allAlbumTracks = await Helpers.MusicBrainzHelper.GetArtistTracks(entity.Artist);

                    tracks.AddRange(allAlbumTracks.Select(t => new Track
                    {
                        Id = Guid.Parse(t.Id),
                        Artist = entity.Artist,
                        Title = string.Format("{0} - {1}", entity.Artist, t.Title),
                        Duration = t.Length
                    }));
                }
                catch
                {
                    continue;
                }
            }

            return tracks;
        }

        #endregion
    }
}