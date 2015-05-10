using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Store.Models
{
    public class Playlist : BaseEntity
    {
        public string Name { get; set; }

        [Index]
        public string Hash { get; set; }

        public string HashDescription { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
        public virtual ICollection<Station> Stations { get; set; } 
    }
}
