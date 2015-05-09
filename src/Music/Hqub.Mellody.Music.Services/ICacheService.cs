using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Store.Models;

namespace Hqub.Mellody.Music.Services
{
    public interface ICacheService
    {
        bool Exists(List<Poco.QueryEntity> query);
        Playlist GetPlaylist(List<Poco.QueryEntity> query);
        void AddPlaylist(List<Poco.QueryEntity> query, Guid playlistId);
        
    }
}
