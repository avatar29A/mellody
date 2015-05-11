using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Store.Models
{
    /// <summary>
    /// Station entity
    /// </summary>
    public class Station : BaseEntity
    {
        public Station()
        {
            Playlists = new Collection<Playlist>();
        }

        /// <summary>
        /// Station name. Generated on the basis of playlists names.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Playlist collection.
        /// </summary>
        public virtual ICollection<Playlist> Playlists { get; set; } 
    }
}
