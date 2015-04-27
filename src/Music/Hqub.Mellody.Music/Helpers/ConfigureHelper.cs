using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Helpers
{
    public static class ConfigureHelper
    {
        public static Configure.PythonConfigureSection GetAuthConfigure()
        {
            var config = (Configure.PythonConfigureSection)
                System.Configuration.ConfigurationManager.GetSection("customSectionGroup/authSection");

            return config;
        }
    }
}
