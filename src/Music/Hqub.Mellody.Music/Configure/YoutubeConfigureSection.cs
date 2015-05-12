using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Configure
{
    public class YoutubeConfigureSection : ConfigurationSection
    {
        #region Keys

        public const string ApiKeyName = "apiKey";
        public const string AppKeyName = "applicationName";
        public const string MaxResultKeyName = "maxResults";

        #endregion

        [ConfigurationProperty(ApiKeyName, DefaultValue = "", IsRequired = true)]
        public string ApiKey
        {
            get { return (string) this[ApiKeyName]; }
            set { this[ApiKeyName] = value; }
        }

        [ConfigurationProperty(AppKeyName, DefaultValue = "", IsRequired = true)]
        public string ApplicationName
        {
            get { return (string) this[AppKeyName]; }
            set { this[AppKeyName] = value; }
        }

        [ConfigurationProperty(MaxResultKeyName, DefaultValue = 10)]
        public int MaxResults
        {
            get { return (int) this[MaxResultKeyName]; }
            set { this[MaxResultKeyName] = value; }
        }
    }
}
