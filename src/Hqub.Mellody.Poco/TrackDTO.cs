using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Poco
{
    /// <summary>
    /// DTO object for Track entity. (see Store project)
    /// </summary>
    public class TrackDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Artist { get; set; }
        public int Duration { get; set; }
        public int Quality { get; set; }

        /// <summary>
        /// Url for artist photo
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Genres, associations with track
        /// </summary>
        public List<string> Tags { get; set; } 

        /// <summary>
        /// Biography artist
        /// </summary>
        public string ArtistBio { get; set; }

        public string VideoId { get; set; }

        public List<ArtistDTO> SimilarArtists { get; set; }

        public string FullTitle { get { return ToString(); } }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Artist, Title);
        }
    }
}
