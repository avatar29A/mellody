using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hqub.Mellody.Web.Models
{
    public class Track
    {
        public string Artist { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }
}