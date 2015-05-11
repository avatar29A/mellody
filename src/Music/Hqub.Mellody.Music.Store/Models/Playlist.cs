using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Store.Models
{
    /// <summary>
    /// Plalist entity.
    /// </summary>
    public class Playlist : BaseEntity
    {
        /// <summary>
        /// Playlist name. Generated on the basis of artists names.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unique value to search for similar playlists.
        /// </summary>
        [Index]
        public string Hash { get; set; }

        /// <summary>
        /// Contains text on the basic of generated hash.
        /// </summary>
        public string HashDescription { get; set; }

        /// <summary>
        /// Tracks collection.
        /// </summary>
        public virtual ICollection<Track> Tracks { get; set; }

        /// <summary>
        /// Station collection.
        /// </summary>
        public virtual ICollection<Station> Stations { get; set; } 
    }
}
