using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Core;

namespace Hqub.Mellody.HostService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            MellodyBotStart();
        }

        protected override void OnStop()
        {
        }

        private string GetToken()
        {
            var authConfigure = Core.Helpers.ConfigureHelper.GetAuthConfigure();

            var r = Core.Utilities.PythonInvoker.Execute(authConfigure.PythonPath, new List<string>
            {
                authConfigure.ScriptName,
                authConfigure.AppId,
                authConfigure.Email,
                authConfigure.Password,
                authConfigure.Scope
            });

            return r;
        }

        private void MellodyBotStart()
        {
            var token = GetToken();

            try
            {
                RunBot(token);
            }
            catch (Exception)
            {
                RunBot(token);
            }
        }

        private static void RunBot(string token)
        {
            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var bot = new MellodyBot(api);
            bot.Live();
        }
    }
}
