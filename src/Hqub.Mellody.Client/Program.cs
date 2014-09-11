using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Hqub.Mellody.Client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GetMyAudioRecords();

            Console.ReadKey();
        }

        public static void GetMyAudioRecords()
        {
            var token = GetToken();

            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var audio = api.GetAudioProduct();

            var response = audio.Get("6666100");

            var counter = 0;
            foreach (var track in response.Tracks)
            {
                Console.WriteLine("{0}. {1} - {2}", ++counter, track.Artist, track.Title);
            }
        }

        public static void SearchScorpions()
        {
            var token = GetToken();

            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var audio = api.GetAudioProduct();

            var response = audio.Search("Scorpions");

            var counter = 0;
            foreach (var track in response.Tracks)
            {
                Console.WriteLine("{0}. {1} - {2}", ++counter, track.Artist, track.Title);
            }
        }

        public static string GetToken()
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
    }
}
