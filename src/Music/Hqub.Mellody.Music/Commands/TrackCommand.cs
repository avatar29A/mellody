using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Commands
{
    public class TrackCommand : ICommand
    {
        public TrackCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name
        {
            get { return "TrackCommand"; }
        }
        public List<Entity> Entities { get; set; }
    }
}
