using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Store.Models
{
    public class Station : BaseEntity
    {
        public Station()
        {
            Playlists = new Collection<Playlist>();
        }

        public string Name { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; } 
    }
}
