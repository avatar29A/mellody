using System;
using System.Collections.Generic;

namespace Hqub.Mellody.Poco
{
    /// <summary>
    /// DTO object for Playlist entity. (see Store project)
    /// </summary>
    public class PlaylistDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Hash { get; set; }

        public string HashDescription { get; set; }
    }
}
