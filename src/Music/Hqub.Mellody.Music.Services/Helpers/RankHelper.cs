using System;
using System.Collections.Generic;
using System.Linq;

namespace Hqub.Mellody.Music.Services
{
    public static class RankHelper
    {
        public static int Calc(string query, string title)
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

            if (IsCover(title))
                rank -= 100;

            return rank;
        }

        private static bool IsCover(string title)
        {
            var listCoverWords = new List<string>
            {
                "cover",
                "кавер",
                "разбор",
                "концерт"
            };

            return listCoverWords.Any(title.Contains);
        }
    }
}
