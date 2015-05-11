using System;
using Newtonsoft.Json;

namespace Hqub.Mellody.Web.Models.Response
{
    /// <summary>
    /// Return the entity on request of creating station.
    /// </summary>
    [JsonObject]
    public class RadioCreatedResponse : ResponseEntity
    {
        public RadioCreatedResponse()
        {
            
        }

        public RadioCreatedResponse(Guid stationId)
        {
            StationId = stationId;
        }

        [JsonProperty("stationId")]
        public Guid StationId { get; set; }
    }
}