using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hqub.Mellody.Web.Models.Response
{
    public class ResponseEntity
    {
        /// <summary>
        /// Error code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Indicate is request has error
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Description the problems
        /// </summary>
        public string Description { get; set; }
    }
}