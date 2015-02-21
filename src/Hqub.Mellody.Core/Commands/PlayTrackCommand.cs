using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Core.Commands
{
    public class PlayTrackCommand : ICommand
    {
        public PlayTrackCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name
        {
            get { return "PlayTrackCommand"; }
        }
        public List<Entity> Entities { get; set; }
    }
}
