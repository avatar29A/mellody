using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Services
{
    public interface IConfigurationService
    {
        Configure.LastfmConfigureSection GetLastfmConfig();
        Configure.YoutubeConfigureSection GetYoutubeConfig();
    }
}
