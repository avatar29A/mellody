using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Store.Models;

namespace Hqub.Mellody.Music.Services
{
    public interface IPlaylistService
    {
        /// <summary>
        /// Create music statio
        /// </summary>
        /// <returns></returns>
        Task<Playlist> CreatePlaylist(Hqub.Mellody.Poco.QueryEntity queries);

        List<Track> GetPlaylist(Guid playlistId);
    }
}
