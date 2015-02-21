using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Core.Commands
{
    public class PlayAlbumCommand : ICommand
    {
        public PlayAlbumCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name
        {
            get { return "PlayAlbumCommand"; }
        }

        public List<Entity> Entities { get; set; }
    }
}
