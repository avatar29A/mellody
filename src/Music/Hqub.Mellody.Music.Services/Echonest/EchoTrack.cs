using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hqub.Mellody.Music.Services.Echonest
{
    [XmlRoot("song")]
    public class EchoTrack
    {
        [XmlElement("artist_id")]
        public string ArtistId { get; set; }

        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("artist_name")]
        public string ArtistName { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }
    }
}
