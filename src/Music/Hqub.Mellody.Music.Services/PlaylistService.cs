using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Commands;
using Hqub.Mellody.Music.Services.Exceptions;
using Hqub.Mellody.Music.Store;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly CommandFactory _mellodyTranslator;
        private readonly Dictionary<Type, Func<List<Entity>, Task<List<Track>>>> _mappingCommand; 

        public PlaylistService()
        {
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

        public Dictionary<Type, Func<List<Entity>, Task<List<Track>>>> MappingCommand
        {
            get { return _mappingCommand; }
        }

        public bool CheckPlaylist(List<QueryEntity> queries)
        {
            return CheckQueries(queries);
        }

        public async Task<List<Track>> CreatePlaylist(List<QueryEntity> queries)
        {
            if (!CheckQueries(queries))
                throw new Exception();

            var playlist = new List<Track>();

            foreach (var query in queries)
            {
                var queryText = GetQueryText(query);

                var command = _mellodyTranslator.Create(queryText);
                if (command == null)
                    continue;

                playlist.AddRange(await _mappingCommand[command.GetType()](command.Entities));
            }

            if(playlist.Count == 0)
                throw new EmptySearchResultException();

            return playlist;
        }

        public List<Track> GetPlaylist(Guid playlistId)
        {
            using (var context = new MusicStoreDbContext())
            {
                return context.Tracks.Where(t => t.Playlist.Id == playlistId).Select(t => new Track
                {
                    Artist = t.Artist,
                    Title = t.Title,
                    Duration = t.Duration,
                    Id = t.Id
                }).ToList();
            }
        }

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
                    var allAlbumTracks = await Music.Helpers.MusicBrainzHelper.GetArtistTracks(entity.Artist);

                    tracks.AddRange(allAlbumTracks.Select(t => new Track
                    {
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

        private string GetArtist(string title)
        {
            var splitTitle = title.Split('-');

            if (splitTitle.Length != 2)
            {
                return string.Empty;
            }

            return splitTitle[0].Trim();
        }

        private bool CheckQueries(List<QueryEntity> queries)
        {
            return true;
        }
    }
}