using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Services.Configure
{
    public class YoutubeConfig
    {
        public YoutubeConfig()
        {
            MaxResults = 10;
        }

        public string DeveloperKey { get; set; }
        public string ApplicationName { get; set; }
        public int MaxResults { get; set; }
    }
}
