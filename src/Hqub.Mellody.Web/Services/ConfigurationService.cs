using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hqub.Mellody.Music.Configure;
using Hqub.Mellody.Music.Services;

namespace Hqub.Mellody.Web.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public LastfmConfigureSection GetLastfmConfig()
        {
            throw new NotImplementedException();
        }

        public YoutubeConfigureSection GetYoutubeConfig()
        {
            var config = (YoutubeConfigureSection)
               System.Configuration.ConfigurationManager.GetSection("customSectionGroup/youtubeSection");

            return config;
        }
    }
}