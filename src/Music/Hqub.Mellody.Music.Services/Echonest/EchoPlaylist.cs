using System.Collections.Generic;
using System.Xml.Serialization;

namespace Hqub.Mellody.Music.Services.Echonest
{
    [XmlRoot("response")]
    public class EchoPlaylist
    {
        [XmlArray("songs")]
        [XmlArrayItem("song")]
        public List<EchoTrack> Tracks { get; set; }

        public static EchoPlaylist Empty()
        {
            return new EchoPlaylist
            {
                Tracks = new List<EchoTrack>()
            };
        }
    }
}
