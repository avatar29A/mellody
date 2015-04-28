using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Commands
{
    public class ArtistCommand : ICommand
    {
        public ArtistCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name
        {
            get { return "ArtistCommand"; }
        }

        public List<Entity> Entities { get; set; }
    }
}
