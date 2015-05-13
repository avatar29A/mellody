using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Web.Models.Response
{
    public class PlaylistResponse : ResponseEntity
    {
        public List<TrackDTO> Tracks { get; set; }
    }
}