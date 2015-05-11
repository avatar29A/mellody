using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Store.Models
{
    /// <summary>
    /// Base entity. Contains primary key and date modify attributes.
    /// </summary>
    public abstract class BaseEntity : IHasId
    {
        protected BaseEntity()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        
        public Guid Id { get; set; }
    }
}
