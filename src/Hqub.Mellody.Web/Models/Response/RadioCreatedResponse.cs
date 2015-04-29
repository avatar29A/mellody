using System;
using Newtonsoft.Json;

namespace Hqub.Mellody.Web.Models.Response
{
    [JsonObject]
    public class RadioCreatedResponse : ResponseEntity
    {
        [JsonProperty("stationId")]
        public Guid StationId { get; set; }
    }
}