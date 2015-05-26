using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Commands
{
    public class GenreCommand : ICommand
    {
        public GenreCommand()
        {
            Entities = new List<Entity>();
        }

        public string Name { get { return "GenreCommand"; } }
        public List<Entity> Entities { get; set; }
    }
}
