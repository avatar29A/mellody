using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Commands
{
    public class HelpCommand : ICommand
    {
        public string Name
        {
            get { return "HelpCommand"; }
        }

        public List<Entity> Entities { get; set; }
    }
}
