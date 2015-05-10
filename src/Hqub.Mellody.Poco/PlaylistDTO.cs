using System.Collections.Generic;

namespace Hqub.Mellody.Poco
{
    public class PlaylistDTO
    {
        public string Name { get; set; }

        public string Hash { get; set; }

        public string HashDescription { get; set; }

        public List<TrackDTO> Tracks { get; set; }
    }
}
