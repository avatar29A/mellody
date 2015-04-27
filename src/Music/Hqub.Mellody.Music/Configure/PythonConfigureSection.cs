using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Configure
{
    public class PythonConfigureSection : ConfigurationSection
    {
        [ConfigurationProperty("pythonPath", DefaultValue = "python.exe", IsRequired = true)]
        public string PythonPath
        {
            get { return (string)this["pythonPath"]; }
            set {  this["pythonPath"] = value; }
        }

        [ConfigurationProperty("scriptName", DefaultValue = "", IsRequired = true)]
        public string ScriptName
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + (string) this["scriptName"]; }
            set { this["scriptName"] = value; }
        }

        [ConfigurationProperty("appId", DefaultValue = "", IsRequired = true)]
        public string AppId
        {
            get { return (string) this["appId"]; }
            set { this["appId"] = value; }
        }

        [ConfigurationProperty("email", DefaultValue = "", IsRequired = true)]
        public string Email
        {
            get { return (string)this["email"]; }
            set { this["email"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("scope", DefaultValue = "", IsRequired = true)]
        public string Scope
        {
            get { return (string)this["scope"]; }
            set { this["scope"] = value; }
        }
    }
}
