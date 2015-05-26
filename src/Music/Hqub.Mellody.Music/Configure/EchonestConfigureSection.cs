using System.Configuration;

namespace Hqub.Mellody.Music.Configure
{
    public class EchonestConfigureSection : ConfigurationSection
    {
        public const string BaseUrlKeyName = "baseUrl";
        public const string AccessTokenKeyName = "accessToken";

        [ConfigurationProperty(BaseUrlKeyName)]
        public string BaseUrl
        {
            get { return (string) this[BaseUrlKeyName]; }
            set { this[BaseUrlKeyName] = value; }
        }

        [ConfigurationProperty(AccessTokenKeyName)]
        public string AccessToken
        {
            get { return (string) this[AccessTokenKeyName]; }
            set { this[AccessTokenKeyName] = value; }
        }
    }
}
