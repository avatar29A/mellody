using System.Collections.Generic;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Web.Models.Response
{
    public class GetHistoryStationsResponse : ResponseEntity
    {
        public GetHistoryStationsResponse(IEnumerable<StationDTO> stations)
        {
            Stations = new List<StationDTO>(stations);
        }

        public List<StationDTO> Stations { get; set; } 
    }
}