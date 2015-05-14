using System;
using System.Collections.Generic;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    public interface IStationService
    {
        /// <summary>
        /// Create new music station
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        Guid Create(List<Playlist> tracks);

        /// <summary>
        /// Get station entity from db
        /// </summary>
        /// <param name="id">Station ID</param>
        /// <returns>Station</returns>
        Station Get(Guid id);

        /// <summary>
        /// Get tracks from station.
        /// </summary>
        /// <param name="stationId">ID station</param>
        /// <returns></returns>
        IList<Track> GetTracks(Guid stationId);

        /// <summary>
        /// Get station name.
        /// </summary>
        /// <param name="id">Station ID</param>
        /// <returns>reurn station name</returns>
        string GetName(Guid id);
    }
}
