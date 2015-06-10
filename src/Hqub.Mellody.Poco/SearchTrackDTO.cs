namespace Hqub.Mellody.Poco
{
    /// <summary>
    /// Poco object for youtube, vk search API.
    /// </summary>
    public class SearchTrackDTO
    {
        public SearchTrackDTO(string title, string id, int rank = 0)
        {
            Id = id;
            Title = title;
            Rank = rank;
        }

        public SearchTrackDTO(string title, string id, string url, int rank = 0)
            : this(title, id, rank)
        {
            Url = url;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Url { get; set; }

        public int Rank { get; set; }

        public string ThumbnaillUrlStandard { get; set; }
    }
}
