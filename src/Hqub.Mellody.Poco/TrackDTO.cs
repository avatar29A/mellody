using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Poco
{
    /// <summary>
    /// DTO object for Track entity. (see Store project)
    /// </summary>
    public class TrackDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Artist { get; set; }
        public int Duration { get; set; }
        public int Quality { get; set; }

        public YoutubeVideoDTO YoutubeInfo { get; set; }
    }
}
