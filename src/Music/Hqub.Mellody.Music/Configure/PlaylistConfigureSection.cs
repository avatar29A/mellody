using System.Configuration;

namespace Hqub.Mellody.Music.Configure
{
    public class PlaylistConfigureSection : ConfigurationSection
    {
        public const string MaxTracksKeyName = "maxTracks";

        /// <summary>
        /// Max amount tracks on station. (Required for load tracks by Artist)
        /// </summary>
        [ConfigurationProperty(MaxTracksKeyName, DefaultValue = 100)]
        public int MaxTracks
        {
            get { return (int)this[MaxTracksKeyName]; }
            set { this[MaxTracksKeyName] = value; }
        }
    }
}
