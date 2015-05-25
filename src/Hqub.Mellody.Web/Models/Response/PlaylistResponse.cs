using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Web.Models.Response
{
    public class PlaylistResponse : ResponseEntity
    {
        public string StationName { get; set; }

        public List<TrackDTO> Tracks { get; set; }

        public List<TrackDTO> HistoryTracks { get; set; }

        public List<StationDTO> HistoryStations { get; set; } 
    }
}