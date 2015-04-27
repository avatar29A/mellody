using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Commands
{
    public interface ICommand
    {
        string Name { get; }
        List<Entity> Entities { get; set; } 
    }
}
