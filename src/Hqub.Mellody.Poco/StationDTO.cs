using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Poco
{
    public class StationDTO
    {
        public string Name { get; set; }
        public List<PlaylistDTO> Playlists { get; set; } 
    }
}
