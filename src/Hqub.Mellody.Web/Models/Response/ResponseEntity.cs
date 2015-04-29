using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Hqub.Mellody.Web.Models.Response
{
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
        /// Description the problems
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}