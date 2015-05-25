using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Poco
{
    /// <summary>
    /// DTO object for Station entity. (see Store project)
    /// </summary>
    public class StationDTO
    {
        private const int MaxNameLength = 10;

        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ShortName
        {
            get { return string.Format("{0} ...", string.Join("", Name.Take(MaxNameLength))); }
        }
    }
}
