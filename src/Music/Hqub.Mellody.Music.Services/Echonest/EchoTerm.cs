using System.Xml.Serialization;

namespace Hqub.Mellody.Music.Services.Echonest
{

    [XmlRoot("term")]
    public class EchoTerm
    {
        [XmlElement("frequency")]
        public float Frequency { get; set; }

        [XmlElement("name")]
        public string Name { get; set;  }

        [XmlElement("weight")]
        public float Weight { get; set; }
    }
}