using System;
using System.Collections.Generic;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    public interface IStationService
    {
        Guid Create(List<Track> tracks);
    }
}
