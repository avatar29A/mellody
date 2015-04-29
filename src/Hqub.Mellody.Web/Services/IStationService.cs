using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Web.Services
{
    public interface IStationService
    {
        Guid Create(List<Models.Track> tracks);
    }
}
