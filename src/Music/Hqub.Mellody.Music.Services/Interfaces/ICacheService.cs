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
        bool Exists(QueryEntity query);
        Playlist GetPlaylist(QueryEntity query);
        void AddPlaylist(QueryEntity query, Guid playlistId);
        Playlist AddPlaylist(QueryEntity query, Playlist playlist);
    }
}
