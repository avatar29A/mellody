using System.Collections.Generic;
using Hqub.Mellody.Poco;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hqub.Mellody.Web.Models
{
    /// <summary>
    /// ViewModel for creating playlist.
    /// </summary>
    [JsonObject]
    public class PrepareRadioModel
    {
        /// <summary>
        /// List of queries.
        /// </summary>
        public List<QueryEntity> Queries { get; set; }
    }
}