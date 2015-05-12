using System.Configuration;

namespace Hqub.Mellody.Music.Configure
{
    public class LastfmConfigureSection : ConfigurationSection
    {
        #region Keys

        public const string ApiKeyName = "apiKey";
        public const string ApiSecretName = "apiSecret";

        #endregion

        [ConfigurationProperty(ApiKeyName, DefaultValue = "", IsRequired = true)]
        public string ApiKey
        {
            get { return (string) this[ApiKeyName]; }
            set { this[ApiKeyName] = value; }
        }

        [ConfigurationProperty(ApiSecretName, DefaultValue = "", IsRequired = true)]
        public string ApiSecret
        {
            get { return (string) this[ApiSecretName]; }
            set { this[ApiSecretName] = value; }
        }
    }
}
