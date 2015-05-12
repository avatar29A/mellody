using System.Collections.Generic;
using System.Linq;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Hqub.Mellody.Music.Services.Configure;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    /// <summary>
    /// Service for integration with youtube API.
    /// </summary>
    public class YoutubeService : IYoutubeService
    {
        private readonly IConfigurationService _configurationService;
        private readonly YoutubeConfig _config;

        public YoutubeService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _config = _configurationService.GetYoutubeConfig();
        }

        public List<YoutubeVideoDTO> Search(string query)
        {
            var youtube = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = _config.DeveloperKey,
                ApplicationName = _config.ApplicationName
            });

            var searchListRequest = youtube.Search.List("snippet");
            searchListRequest.Q = query;
            searchListRequest.MaxResults = _config.MaxResults;

            var searchListResponse = searchListRequest.Execute();

            return (from searchResult in searchListResponse.Items
                where searchResult.Id.Kind == "youtube#video"
                select new YoutubeVideoDTO(searchResult.Snippet.Title, searchResult.Id.VideoId)
                {
                    ThumbnaillUrlStandard = searchResult.Snippet.Thumbnails.Standard.Url
                }).ToList();
        }
    }
}
