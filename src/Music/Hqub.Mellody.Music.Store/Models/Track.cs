using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hqub.Mellody.Music.Store.Models
{
    public class Track : BaseEntity
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Duration { get; set; }
        public int Quality { get; set; }

        public int Position { get; set; }

        [Index]
        public Guid MbId { get; set; }

        public virtual Playlist Playlist { get; set; }
    }
}