using System;
using Newtonsoft.Json;

namespace Hqub.Mellody.Web.Models.Response
{
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