using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Auth();
            Console.ReadKey();
        }

        public static void Auth()
        {
            Console.WriteLine("Start auth");

            var authConfigure = Core.Helpers.ConfigureHelper.GetAuthConfigure();

            var r = Core.Utilities.PythonInvoker.Execute(authConfigure.PythonPath, new List<string>
            {
                authConfigure.ScriptName,
                authConfigure.AppId,
                authConfigure.Email,
                authConfigure.Password,
                authConfigure.Scope
            });

            Console.WriteLine(r);
        }
    }
}
