using System.Collections.Generic;
using Hqub.Mellody.Web.Models.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hqub.Mellody.Web.Models
{
    [JsonObject]
    public class PrepareRadioModel
    {
        public List<QueryEntity> Queries { get; set; }
    }
}