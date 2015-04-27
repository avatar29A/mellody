using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Cache;
using Hqub.Mellody.Music.Commands;
using Hqub.Mellody.Music.Helpers;
using Hqub.Mellowave.Vkontakte.API.Factories;
using Hqub.Mellowave.Vkontakte.API.LongPoll;
using Hqub.Mellowave.Vkontakte.API.Model.Audio;
using Hqub.MusicBrainz.API.Entities;

namespace Hqub.Mellody.Music
{
    public class MellodyBot : IDisposable
    {
        const int MaxCountTrackOnDisk = 10;

        private readonly ApiFactory _vk;
        private readonly CommandFactory _mellodyTranslator;
        private LongPollServer _vkTunnel;
        private MellodyMemory _mellodyMemory;

        public MellodyBot(ApiFactory vk)
        {
            _vk = vk;
            _mellodyTranslator = new CommandFactory();
            _mellodyMemory = new MellodyMemory();
        }

        public void Live()
        {
            _vkTunnel = LongPollServer.Connect(_vk);
            
#if DEBUG
            _vkTunnel.ReceiveData += Console.WriteLine;
#endif

            _vkTunnel.ReceiveMessage += TryReceiveMessage;
        }

        private void TryReceiveMessage(int messageId, int fromId, DateTime timestamp, string subject, string text)
        {
            try
            {
                ReceiveMessage(messageId, fromId, timestamp, subject, text);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }
        }

        private void ReceiveMessage(int messageId, int fromId, DateTime timestamp, string subject, string text)
        {
            if (text.Contains("[mellody]")) 
                return;

            var command = _mellodyTranslator.Create(text.Replace("&quot;", "\""));
            if(command == null)
                return;

            switch (command.Name)
            {
                case "PlayArtistCommand":
                    break;
                case "InfoArtistCommand":
                    SendInfoArtistCommand(fromId, (InfoArtistCommand) command);
                    break;
                case "PlayAlbumCommand":
                    SendPlayAlbumCommand(fromId, (PlayAlbumCommand) command);
                    break;
                case "PlayTrackCommand":
                    SendPlayTrackCommand(fromId, (PlayTrackCommand) command);
                    break;
                case "InfoAlbumCommand":
                    SendInfoAlbumCommand(fromId, (InfoAlbumCommand) command);
                    break;
                case "HelpCommand":
                    SendHelpCommand(fromId); 
                    break;
            }
        }

        #region Commands

        private void SendHelpCommand(int userId)
        {
            var answer = new StringBuilder();

            answer.AppendLine("Извини, я не поняла :(\n");

            answer.AppendLine("Доступные команды:\n");

            answer.AppendLine("[Команды для поиска песен]");
            answer.AppendLine("1. трек \"Король и Шут - Кукла Колдуна\"");
            answer.AppendLine("2. треки \"Король и Шут - Бедняжка\" \"Ozzy Osbourne - Dreamer\"");
            answer.AppendLine("3. альбом \"Ария -  Ночь короче дня\"");
            answer.AppendLine("4. группы \"Ария\" \"Кукрыниксы\"");

            answer.AppendLine();

            answer.AppendLine("[Команды для получения справки]");
            answer.AppendLine("1. \"Король и Шут\" инфо");
            answer.AppendLine("2. альбом \"Король и Шут - Камнем по голове\" инфо");


            SendMessage(userId, answer.ToString());
        }

        private async void SendInfoArtistCommand(int userId, InfoArtistCommand command)
        {
            var message = new StringBuilder();

            foreach (var entity in command.Entities)
            {
                var artist = await MusicBrainzHelper.GetArtistInfo(entity.Artist);

                message.AppendLine(artist.ToString());

                SendMessage(userId, message.ToString());
                message.Clear();
            }
        }

        private async void SendInfoAlbumCommand(int fromId, InfoAlbumCommand command)
        {
            var message = new StringBuilder();

            foreach (var entity in command.Entities)
            {
                var albumDTO = await Helpers.MusicBrainzHelper.GetAlbumTracks(entity.Artist, entity.Album);
                var recordings = albumDTO.Tracks;

                message.AppendLine(albumDTO.ToString());
                message.AppendLine();

                for (int i = 0; i < recordings.Count; i++)
                {
                    var recording = recordings[i];
                    message.AppendFormat("{0}. {1} [{2}]\n", i + 1, recording.Title, TimeSpan.FromMilliseconds(recording.Length).ToString("m\\:ss"));
                }

                SendMessage(fromId, message.ToString());
                message.Clear();
            }

        }

        private void SendPlayTrackCommand(int userId, PlayTrackCommand command)
        {
            var message = new StringBuilder();

            var attachment = new List<string>();

            var audio = _vk.GetAudioProduct();
            foreach (var entity in command.Entities)
            {
                var response = audio.Search(string.Format("{0}-{1}", entity.Artist, entity.Track), count: 1);
                if (response.Tracks.Count == 0)
                    continue;

                var track = response.Tracks[0];
                attachment.Add(string.Format("audio{0}_{1}", track.OwnerId, track.Id));
            }

            message.AppendLine(attachment.Count > 0 ? "Вот, что нашла" : "Увы, ничего не найдено :(");
            
            //TODO: Нужно бить сообщения на части по 10 трэков
            SendMessage(userId, message.ToString(), string.Join(",", attachment));
        }

        private async void SendPlayAlbumCommand(int userId, PlayAlbumCommand command)
        {
            var message = new StringBuilder();
            var attachment = new List<string>();

            SendPrepareRequest(userId);
            
            foreach (var entity in command.Entities)
            {
                var albumDTO = await Helpers.MusicBrainzHelper.GetAlbumTracks(entity.Artist, entity.Album);
                var recordings = albumDTO.Tracks;

                var amountDiscs = GetAmountDiscs(recordings.Count);

                message.AppendLine(albumDTO.ToString());

                var tracks = GetTracksFromVk(albumDTO.Artist, recordings);

                // Делим треки по дискам (в вк ограничение на 10 треков в сообщении)
                for (int discI = 0; discI < amountDiscs; ++discI)
                {
                    attachment.AddRange(
                        tracks.Skip(discI*MaxCountTrackOnDisk)
                            .Take(MaxCountTrackOnDisk)
                            .Select(t => string.Format("audio{0}_{1}", t.OwnerId, t.Id)));

                    if (attachment.Count == 0)
                        continue;

                    message.AppendFormat("\nДиск {0}", discI + 1);
                    SendMessage(userId, message.ToString(), string.Join(",", attachment));

                    message.Clear();
                    attachment.Clear();
                }
            }
        }

        private List<Audio> GetTracksFromVk(string artistName, List<Recording> recordings)
        {
            var audio = _vk.GetAudioProduct();
            const int max = 3;

            var tracks = new List<Audio>();
            for (int i = 0; i < (int) Math.Ceiling(recordings.Count*1.0/max); i++)
            {
                tracks.AddRange(
                    audio.SearchMany(
                        recordings.Skip(i*max)
                            .Take(max)
                            .Select(r => string.Format("{0}-{1}", artistName, r.Title))
                            .ToList()));

                //2 запроса в секунду
                if (i%2 == 0)
                    Thread.Sleep(1000);
            }

            return tracks;
        }

        /// <summary>
        /// Сказать пользователю, что его запрос обрабатывается
        /// </summary>
        private void SendPrepareRequest(int userId)
        {
            SendMessage(userId, "Обрабатываю запрос. Пожалуйста ожидайте.");
        }

        /// <summary>
        /// Получаем кол-во треков на одной стороне
        /// </summary>
        /// <returns></returns>
        private int GetAmountDiscs(int trackAmount)
        {
            if (trackAmount <= MaxCountTrackOnDisk)
                return MaxCountTrackOnDisk;

            var sideCount = Math.Ceiling(trackAmount * 1.0 / MaxCountTrackOnDisk);

            return (int)sideCount;
        }

        #endregion


        private void SendMessage(int userId, string text, string attachment = "")
        {
            var message = _vk.GetMessageProduct();

            message.Send(userId, message: string.Format("{0}\n", text), attachment: attachment);
        }

        public void Dispose()
        {
            _vkTunnel.ReceiveMessage -= ReceiveMessage;
        }
    }
}
