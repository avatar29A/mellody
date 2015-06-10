using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hqub.Mellody.Poco
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum SourceTypeEnum
    {
        Youtube,
        Vkontakte
    }
}
