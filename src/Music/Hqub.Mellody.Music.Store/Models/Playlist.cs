using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Store.Models
{
    public class Playlist : IHasId
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Index]
        public string Hash { get; set; }


        public virtual ICollection<Track> Tracks { get; set; }
    }
}
