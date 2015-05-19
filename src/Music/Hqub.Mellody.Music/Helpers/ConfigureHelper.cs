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
