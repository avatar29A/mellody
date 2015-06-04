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
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services.Implementation
{
    public class LastfmPlaylistService : IPlaylistService
    {
        #region Fields

        private readonly ILogService _logService;
        private readonly PlaylistConfigureSection _configure;
        private readonly ICacheService _cacheService;
        private readonly IEchonestService _echonestService;

        private readonly Dictionary<TypeQuery, Func<QueryEntity, Task<List<Track>>>> _mappingCommand;

        #endregion

        #region .ctor

        public LastfmPlaylistService(ILogService logService,
            IConfigurationService configurationService,
            ICacheService cacheService,
            IEchonestService echonestService)
        {
            _logService = logService;
            _configure = configurationService.GetPlaylistConfig();
            _cacheService = cacheService;
            _echonestService = echonestService;

            _mappingCommand = new Dictionary<TypeQuery, Func<QueryEntity, Task<List<Track>>>>
            {

                {
                    TypeQuery.Artist, GetArtists
                },

           
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
            throw new NotImplementedException();
        }

        #region Private Methods

        private async Task<List<Track>> GetArtists(QueryEntity entity)
        {
            var tracks = new List<Track>();

            
            
            return tracks;
        }

        private void LogException(string methodName, IEnumerable<Entity> entities, Exception exception)
        {
            var builder = new StringBuilder(string.Format("PlaylistService.{0}", methodName));
            foreach (var entity1 in entities)
                builder.AppendFormat("\tentity: {0}}\n", entity1.Artist);

            _logService.AddExceptionFull(builder.ToString(), exception);
        }

        #endregion
    }
}
