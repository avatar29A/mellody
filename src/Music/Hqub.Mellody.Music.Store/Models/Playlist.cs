using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Store.Models
{
    public class Playlist : IHasId
    {
        public Guid Id { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
