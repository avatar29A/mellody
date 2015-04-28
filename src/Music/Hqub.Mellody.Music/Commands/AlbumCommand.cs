using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Commands
{
    public class AlbumCommand : ICommand
    {
        public AlbumCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name
        {
            get { return "AlbumCommand"; }
        }

        public List<Entity> Entities { get; set; }
    }
}
