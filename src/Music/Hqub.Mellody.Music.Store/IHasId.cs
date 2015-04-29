using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Store
{
    public interface IHasId
    {
        Guid Id { get; set; }
    }
}
