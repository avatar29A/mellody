using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Services.Configure;

namespace Hqub.Mellody.Music.Client
{
  public  class ConfigureService : Services.IConfigurationService
    {
      public LastfmConfig GetLastfmConfig()
      {
          throw new NotImplementedException();
      }

      public YoutubeConfig GetYoutubeConfig()
      {
          return new YoutubeConfig
          {
              ApplicationName = "mellody",
              DeveloperKey = "AIzaSyDPhkn3ceN2xaVM8xdLwZFCEZzh7-oJRrg",
              MaxResults = 10
          };
      }
    }
}
