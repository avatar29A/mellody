using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Core.Commands
{
    public class PlayArtistCommand : ICommand
    {
        public PlayArtistCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name
        {
            get { return "PlayArtistCommand"; }
        }

        public List<Entity> Entities { get; set; }
    }
}
