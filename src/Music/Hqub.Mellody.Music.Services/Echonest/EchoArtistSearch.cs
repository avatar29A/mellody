using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hqub.Mellody.Music.Services.Echonest
{
    [XmlRoot("response")]
    public class EchoArtistSearch : EchoResponse
    {
        [XmlArray("artists")]
        [XmlArrayItem("artist")]
        public List<EchoArtist> Artists { get; set; } 
    }
}
