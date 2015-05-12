namespace Hqub.Mellody.Poco
{
    /// <summary>
    /// Poco object for youtube search API.
    /// </summary>
    public class YoutubeVideoDTO
    {
        public YoutubeVideoDTO(string title, string videoId)
        {
            VideoId = videoId;
            Title = title;
        }

        public string VideoId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }

        public string ThumbnaillUrlStandard { get; set; }
    }
}
