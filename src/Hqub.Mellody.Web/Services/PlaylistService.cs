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
        private readonly Dictionary<Type, Func<List<Entity>, List<Track>>> _mappingCommand; 

        public PlaylistService()
        {
            _mappingCommand = new Dictionary<Type, Func<List<Entity>, List<Track>>>
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

        public Dictionary<Type, Func<List<Entity>, List<Track>>> MappingCommand
        {
            get { return _mappingCommand; }
        }

        public bool CheckPlaylist(List<QueryEntity> queries)
        {
            return CheckQueries(queries);
        }

        public List<Track> CreatePlaylist(List<QueryEntity> queries)
        {
            if (!CheckQueries(queries))
                throw new QuerySyntaxException();

            var playlist = new List<Track>();

            foreach (var query in queries)
            {
                var command = _mellodyTranslator.Create(query.Name);
                if (command == null)
                    continue;

                playlist.AddRange(_mappingCommand[command.GetType()](command.Entities));
            }

            return playlist;
        }

        private List<Track> GetTracks(List<Entity> entities)
        {
            return entities.Select(t => new Track
            {
                Title = string.Format("{0} - {1}", t.Artist, t.Track)
            }).ToList();
        }


        private List<Track> GetAlbums(List<Entity> entities)
        {
            var tracks = new List<Track>();

            foreach (var entity in entities)
            {
                var task = Music.Helpers.MusicBrainzHelper.GetAlbumTracks(entity.Artist, entity.Album);

                task.Wait();

                var album = task.Result;

                tracks.AddRange(album.Tracks.Select(t => new Track
                {
                    Title = t.Title,
                    Duration = t.Length
                }).ToList());
            }

            return tracks;
        }

        private List<Track> GetArtists(List<Entity> entities)
        {
            return new List<Track>();
        }

        private bool CheckQueries(List<QueryEntity> queries)
        {
            return true;
        }
    }
}