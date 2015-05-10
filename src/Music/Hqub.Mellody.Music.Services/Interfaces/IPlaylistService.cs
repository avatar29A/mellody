using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Store.Models;

namespace Hqub.Mellody.Music.Services
{
    public interface IPlaylistService
    {
        /// <summary>
        /// Create music new playlist
        /// </summary>
        /// <returns>Playlist wrapped in async Task</returns>
        Task<Playlist> Create(Poco.QueryEntity queries);

        /// <summary>
        /// Get track entity from db
        /// </summary>
        /// <param name="id">Track ID</param>
        /// <returns>Track</returns>
        List<Track> Get(Guid id);
    }
}
