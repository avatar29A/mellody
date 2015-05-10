using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Hqub.Mellody.Web.Models.Response
{
    /// <summary>
    /// Basic type of response. Contains information system information.
    /// </summary>
    [JsonObject]
    public class ResponseEntity
    {
        /// <summary>
        /// Error code
        /// </summary>
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        /// <summary>
        /// Indicate is request has error
        /// </summary>
        [JsonProperty("isError")]
        public bool IsError { get; set; }

        /// <summary>
        /// List with descriptions of the errors.
        /// </summary>
        [JsonProperty("errors")]
        public List<string> Errors { get; set; }

            /// <summary>
        /// Message the problems
        /// </summary>
        [JsonProperty("description")]
        public string Message { get; set; }
    }
}