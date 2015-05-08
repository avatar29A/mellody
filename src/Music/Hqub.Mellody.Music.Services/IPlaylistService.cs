using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    public interface IPlaylistService
    {
        /// <summary>
        /// CheckPlaylist queries on syntax error
        /// </summary>
        /// <returns>List with errors</returns>
        bool CheckPlaylist(List<QueryEntity> queries);

        /// <summary>
        /// Create music statio
        /// </summary>
        /// <returns></returns>
        Task<List<Track>> CreatePlaylist(List<QueryEntity> queries);

        List<Track> GetPlaylist(Guid playlistId);
    }
}
