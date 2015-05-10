using System;
using System.Collections.Generic;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    public interface IStationService
    {
        Guid Create(List<Playlist> tracks);
        Station Get(Guid id);
    }
}
