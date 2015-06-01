using System;
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
                        CalcRank(query, searchResult.Snippet.Title))).ToList();
        }

        private int CalcRank(string query, string title)
        {
            var splitTrackName = Helpers.PlaylistHelper.SplitTitle(query);
            var artistName = splitTrackName[0].Trim().ToLower();
            var trackName = splitTrackName[1].Trim().ToLower();
            var clearTitle = title.Trim().ToLower();

            var splitYoutubeTrackName = Helpers.PlaylistHelper.SplitTitle(title);

            var rank = 0;

            if (splitTrackName.Length == 2 && splitYoutubeTrackName.Length == 2)
            {
                if (String.Equals(splitTrackName[0].Trim(), splitYoutubeTrackName[0].Trim(),
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    rank += 70;
                }

                if (String.Equals(splitTrackName[1].Trim(), splitYoutubeTrackName[1].Trim(),
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    rank += 30;
                }
            }
            else
            {
                if (clearTitle.Contains(artistName))
                {
                    rank += 30;
                }

                if (clearTitle.Contains(trackName))
                {
                    rank += 70;
                }
            }

            return rank;
        }
    }
}
