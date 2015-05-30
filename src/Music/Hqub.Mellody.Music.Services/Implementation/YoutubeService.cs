using System.Collections.Generic;
using System.Linq;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Hqub.Mellody.Music.Configure;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    /// <summary>
    /// Service for integration with youtube API.
    /// </summary>
    public class YoutubeService : IYoutubeService
    {
        private readonly YoutubeConfigureSection _config;

        public YoutubeService(IConfigurationService configurationService)
        {
            _config = configurationService.GetYoutubeConfig();
        }

        public List<YoutubeVideoDTO> Search(string query)
        {
            var youtube = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = _config.ApiKey,
                ApplicationName = _config.ApplicationName
            });

            var searchListRequest = youtube.Search.List("snippet");
            searchListRequest.Type = "video";
            searchListRequest.Q = query;
            searchListRequest.VideoDuration = SearchResource.ListRequest.VideoDurationEnum.Medium;

            searchListRequest.MaxResults = _config.MaxResults;

            var searchListResponse = searchListRequest.Execute();

            return (from searchResult in searchListResponse.Items
                where searchResult.Id.Kind == "youtube#video"
                select
                    new YoutubeVideoDTO(searchResult.Snippet.Title, searchResult.Id.VideoId,
                        Helpers.LevenshteinAlgHelper.LevenshteinDistance(query.ToLower(),
                            searchResult.Snippet.Title.ToLower()))).ToList();
        }
    }
}
