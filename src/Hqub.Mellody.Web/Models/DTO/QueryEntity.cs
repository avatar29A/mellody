﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hqub.Mellody.Web.Models.DTO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeQuery
    {
        Artist,
        Query
    }

    /// <summary>
    /// Description query entities. 
    /// </summary>
    [JsonObject]
    public class QueryEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("typeQuery")]
        public TypeQuery TypeQuery { get; set; }
    }
}