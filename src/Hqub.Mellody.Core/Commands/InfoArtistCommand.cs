using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Core.Commands
{
    public class InfoArtistCommand : ICommand
    {
        public InfoArtistCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name
        {
            get { return "InfoArtistCommand"; }
        }

        public List<Entity> Entities { get; set; }
    }
}
