using System;
using System.Collections.Generic;
using System.Net;
using Hqub.Mellody.Music.Commands;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Poco;
using Hqub.Mellowave.Vkontakte.API.LongPoll;
using System.Text.RegularExpressions;

namespace Hqub.Mellody.Music.Client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            Console.WriteLine(TestRegexp("АРия .- Ночь ,короче ;дня!"));

//            CreateTracks();


//            MellodyBotStart();
//            TestGrammar();
//            GetMyAudioRecords();
//            SearchScorpions();
//            GetLongPollServer();
//            SearchScorpions();
//            StartLongPollServer();
//            SendMessage(6666100, "Лови подборку", "audio9203645_80885922,audio4343194_89404022,audio3830978_72952673,audio-21504294_90590072,audio8236081_34095877,audio808376_123428,audio2519124_91781028,audio1761644_71747984,audio4314080_103754952,audio-21186282_88371893,audio18877023_90175747,audio38682_81635782,audio104349233_107043600,audio43280774_84872021");
//            GetDialogs();
//            GetMessages();


            Console.ReadKey();
        }

        private static string TestRegexp(string name)
        {
            return Regex.Replace(name, "[^\\w\\d]", string.Empty).Trim().ToLower();
        }

        private async static void CreateTracks()
        {
            var station = new StationService();
            var playlistService = new PlaylistService(new CacheService());

            var playlist = await playlistService.Create(new QueryEntity
            {
                Name = "Ария - Герой Асфальта",
                TypeQuery = TypeQuery.Album
            });


//            var stationId = station.Create(playlist);

            Console.WriteLine("PocoStation with ID = {0} created.", Guid.Empty);
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            
        }

        public static void MellodyBotStart()
        {
            var token = GetToken();
            Success(string.Format("Токен получен ({0})", token));

            try
            {
                RunBot(token);
                Success("Бот успешно запущен");
            }
            catch (Exception exception)
            {
                Error(exception.Message);
                RunBot(token);
            }
        }

        private static void RunBot(string token)
        {
            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var bot = new MellodyBot(api);
            bot.Live();
        }

        private static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[{1}] {0}", message, DateTime.Now);
            Console.ResetColor();
        }

        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[{1}] {0}", message, DateTime.Now);
            Console.ResetColor();
        }
        

        public static void TestGrammar()
        {
            var fabrica = new CommandFactory();
            var command = fabrica.Create("Слушать альбом \"Корол и Шут - Как в старой сказе\" \"Кукрыниксы - Шаман\"");

            Console.WriteLine(command);
        }

        public static void GetDialogs()
        {
            var token = GetToken();

            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var dialogApi = api.GetMessageProduct();

            var response = dialogApi.GetDialogs();

            foreach (var dialog in response.Dialogs)
            {
                Console.WriteLine("[{0}]\n\n{1}\n", dialog.Message.UserId, dialog.Message.Body);
            }
        }

        public static void GetLongPollServer()
        {
            var token = GetToken();

            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var message = api.GetMessageProduct();

            var response = message.GetLongPollServer();

            Console.WriteLine(response);
        }

        public static void StartLongPollServer()
        {
            var token = GetToken();

            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);

            var longPollServer = LongPollServer.Connect(api);
//            longPollServer.ReceiveData += Console.WriteLine;
            longPollServer.ReceiveMessage += (messageId, fromId, timestamp, subject, text) =>
            {
                Console.WriteLine("[{0}]\n{1}\n", fromId, text);
            };
        }

        public static void GetMessages()
        {
            var token = GetToken();

            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var message = api.GetMessageProduct();

            var response = message.Get();

            foreach (var msg in response.Messages)
            {
                Console.WriteLine("[{0}]\n\n{1}\n", msg.UserId, msg.Body);
            }
        }

        public static void SendMessage(int userId, string text, string attachment="")
        {
            var token = GetToken();

            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var message = api.GetMessageProduct();

            var response = message.Send(userId, message: text, attachment: attachment);

            Console.WriteLine("Message send with ID: {0}", response);
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
                string track_id = string.Format("{0}_{1}", track.OwnerId, track.Id);
                
                Console.WriteLine("{0}. {1} - {2} ({3})", ++counter, track.Artist, track.Title, track_id);
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
                string track_id = string.Format("{0}_{1}", track.OwnerId, track.Id);
                Console.WriteLine("{0}. {1} - {2} ({3})", ++counter, track.Artist, track.Title, track_id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(track.Url);
                Console.ResetColor();
            }
        }

        public static void DownloadUserAudioList(string userId)
        {
            var token = GetToken();

            var api = Mellowave.Vkontakte.API.Factories.ApiFactory.Instance(token);
            var audio = api.GetAudioProduct();

            const int trackAmountPerRequest = 50;

            var amount = audio.GetUserLibraryTrackCount(userId)*1.0;
            var parts = (int) Math.Ceiling(amount/trackAmountPerRequest);

            Console.WriteLine("Amount: {0}", amount);
            Console.WriteLine();

            var trackId = 0;
            for (int i = 0; i < parts; i++)
            {
                var tracks = audio.Get(userId, i*trackAmountPerRequest, trackAmountPerRequest);

                foreach (var track in tracks.Tracks)
                {
                    Console.WriteLine("{1}. {0}", track, ++trackId);
                    try
                    {
                        DownloadTrack(userId, track.ToString(), track.Url);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0} error. {1}", track, ex.Message);
                        Console.ResetColor();
                    }
                }
            }
        }

        private static void DownloadTrack(string ownerId, string trackName, string url)
        {
            var path = "C:\\Temp\\audio\\" + ownerId;
            System.IO.Directory.CreateDirectory(path);

            var web = new WebClient();
            web.DownloadFile(url, string.Format("{0}\\{1}.mp3", path, trackName));
        }

        public static string GetToken()
        {
            var authConfigure = Music.Helpers.ConfigureHelper.GetAuthConfigure();

            var r = Music.Utilities.PythonInvoker.Execute(authConfigure.PythonPath, new List<string>
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
