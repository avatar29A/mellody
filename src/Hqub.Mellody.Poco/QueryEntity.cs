using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hqub.Mellody.Poco
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeQuery
    {
        Artist,
        Album,
        Genre,
        Query
    }

    /// <summary>
    /// Description items from playlist. 
    /// </summary>
    [JsonObject]
    public class QueryEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mbid")]
        public string MusicBrainzId { get; set; }

        [JsonProperty("typeQuery")]
        public TypeQuery TypeQuery { get; set; }
    }
}