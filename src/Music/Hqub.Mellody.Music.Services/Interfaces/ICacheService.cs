using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    public interface ICacheService
    {
        /// <summary>
        /// Is checking exists playlist entity in db by query.
        /// </summary>
        /// <param name="query">Query entity</param>
        /// <returns>It is true, if playlist exists.</returns>
        bool ExistsPlaylist(QueryEntity query);

        /// <summary>
        /// Get playlist from cache.
        /// </summary>
        /// <param name="query">Query entity</param>
        /// <returns>Playlist entity or null, if playlist not found.</returns>
        Playlist GetPlaylist(QueryEntity query);

        /// <summary>
        /// Add plalist into cache.
        /// </summary>
        /// <param name="query">Query entity</param>
        /// <param name="id">Playlist ID</param>
        void AddPlaylist(QueryEntity query, Guid id);

        /// <summary>
        /// Add plalist into cache.
        /// </summary>
        /// <param name="query">Query entity</param>
        /// <param name="playlist">Playlist entity (from db context)</param>
        /// <returns></returns>
        Playlist AddPlaylist(QueryEntity query, Playlist playlist);
    }
}
