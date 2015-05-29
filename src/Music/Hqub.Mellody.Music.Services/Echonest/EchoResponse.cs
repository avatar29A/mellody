using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hqub.Mellody.Music.Services.Echonest
{
    [XmlRoot("response")]
    public abstract class EchoResponse
    {
        [XmlElement("status")]
        public Status Status { get; set; }

        [XmlElement("start")]
        public int Start { get; set; }
    }

    [XmlRoot("status")]
    public class Status
    {
        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("code")]
        public int Code { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }
    }
}
