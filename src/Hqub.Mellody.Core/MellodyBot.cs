using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hqub.Mellody.Core.Cache;
using Hqub.Mellody.Core.Commands;
using Hqub.Mellowave.Vkontakte.API.Factories;
using Hqub.Mellowave.Vkontakte.API.LongPoll;

namespace Hqub.Mellody.Core
{
    public class MellodyBot : IDisposable
    {
        const int MaxCountTrackOnDisk = 9;

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

            _vkTunnel.ReceiveMessage += ReceiveMessage;
        }

        private void ReceiveMessage(int messageId, int fromId, DateTime timestamp, string subject, string text)
        {
            if (text.Contains("[mellody]")) 
                return;

            var command = _mellodyTranslator.Create(text.Replace("&quot;", "\""));
            switch (command.Name)
            {
                case "PlayArtistCommand":
                    break;
                case "PlayAlbumCommand":
                    SendPlayAlbumCommand(fromId, (PlayAlbumCommand) command);
                    break;
                case "PlayTrackCommand":
                    SendPlayTrackCommand(fromId, (PlayTrackCommand) command);
                    break;
                default:
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

            answer.AppendLine("1. Слушать треки");
            answer.AppendLine("2. Слушать исполнителей");
            answer.AppendLine("3. Слушать альбомы");

            answer.AppendLine("\nПример запросов:\n");

            answer.AppendLine("1. найти трек \"Король и Шут - Кукла Колдуна\"");
            answer.AppendLine("2. слушать треки \"Король и Шут - Бедняжка\" \"Ozzy Osbourne - Dreamer\"");
            answer.AppendLine("3. слушать альбом \"Ария -  Ночь короче дня\"");
            answer.AppendLine("4. слушать группы \"Ария\" \"Кукрыниксы\"");

            SendMessage(userId, answer.ToString());
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

            var audio = _vk.GetAudioProduct();
            foreach (var entity in command.Entities)
            {
                var albumDTO = await Helpers.MusicBrainzHelper.GetAlbumTracks(entity.Artist, entity.Album);
                var recordings = albumDTO.Tracks;

                var amountDiscs = GetAmountDiscs(recordings.Count);

                message.AppendLine(albumDTO.ToString());

                // Делим треки по дискам (в вк ограничение на 10 треков в сообщении)
                for (int discI = 0; discI < amountDiscs; ++discI)
                {
                    var tracks =
                        audio.SearchMany(
                            recordings.Skip(discI*MaxCountTrackOnDisk)
                                .Take(MaxCountTrackOnDisk)
                                .Select(r => string.Format("{0}-{1}", entity.Artist, r))
                                .ToList());

                    attachment.AddRange(tracks.Select(t => string.Format("audio{0}_{1}", t.OwnerId, t.Id)));

                    if (attachment.Count == 0)
                        continue;

                    message.AppendFormat("\nДиск {0}", discI + 1);
                    SendMessage(userId, message.ToString(), string.Join(",", attachment));

                    message.Clear();
                    attachment.Clear();
                }
            }
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

            message.Send(userId, message: string.Format("{0}\n[mellody]", text), attachment: attachment);
        }

        public void Dispose()
        {
            _vkTunnel.ReceiveMessage -= ReceiveMessage;
        }
    }
}
