using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Commands
{
    public class InfoAlbumCommand : ICommand
    {
        public InfoAlbumCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name
        {
            get { return "InfoAlbumCommand"; }
        }

        public List<Entity> Entities { get; set; }
    }
}
