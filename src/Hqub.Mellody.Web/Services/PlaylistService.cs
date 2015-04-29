using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Hqub.Mellody.Music.Commands;
using Hqub.Mellody.Web.Exceptions;
using Hqub.Mellody.Web.Models;
using Hqub.Mellody.Web.Models.DTO;

namespace Hqub.Mellody.Web.Services
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
                throw new QuerySyntaxException();

            var playlist = new List<Track>();

            foreach (var query in queries)
            {
                var queryText = GetQueryText(query);

                var command = _mellodyTranslator.Create(queryText);
                if (command == null)
                    continue;

                playlist.AddRange(await _mappingCommand[command.GetType()](command.Entities));
            }

            return playlist;
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
                Title = string.Format("{0} - {1}", t.Artist, t.Track)
            }).ToList();
        }

        private async Task<List<Track>> GetAlbums(List<Entity> entities)
        {
            var tracks = new List<Track>();

            foreach (var entity in entities)
            {
                var album = await Music.Helpers.MusicBrainzHelper.GetAlbumTracks(entity.Artist, entity.Album);

                tracks.AddRange(album.Tracks.Select(t => new Track
                {
                    Title = t.Title,
                    Duration = t.Length
                }).ToList());
            }

            return tracks;
        }

        private async Task<List<Track>> GetArtists(List<Entity> entities)
        {
            var tracks = new List<Track>();

            foreach (var entity in entities)
            {
                var allAlbumTracks = await Music.Helpers.MusicBrainzHelper.GetArtistTracks(entity.Artist);

                tracks.AddRange(allAlbumTracks.Select(t => new Track
                {
                    Title = string.Format("{0} - {1}", entity.Artist, t.Title),
                    Duration = t.Length
                }));
            }

            return tracks;
        }

        private bool CheckQueries(List<QueryEntity> queries)
        {
            return true;
        }
    }
}