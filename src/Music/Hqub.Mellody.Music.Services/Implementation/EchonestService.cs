﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using Hqub.Mellody.Music.Services.Echonest;
using Hqub.Mellody.Music.Services.Interfaces;

namespace Hqub.Mellody.Music.Services.Implementation
{
    public class EchonestService : IEchonestService
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogService _logService;

        public EchonestService(IConfigurationService configurationService, ILogService logService)
        {
            _configurationService = configurationService;
            _logService = logService;
        }

        /// <summary>
        /// Return static radio playlist (constains 100 audio trakcs).
        /// </summary>
        /// <param name="genres">List of genres</param>
        /// <param name="count">Max amount tracks in playlist</param>
        /// <returns>Echonest static playlsit.</returns>
        public EchoPlaylist GetPlaylistByGenre(IEnumerable<string> genres, int count)
        {
            var config = _configurationService.GetEchonestConfig();
            var mergeGenres = string.Join("+", genres);

            var url =
                string.Format("{0}playlist/static?api_key={1}&genre={2}&format=xml&type=genre-radio&results={3}",
                    config.BaseUrl, config.AccessToken, mergeGenres, count);

            try
            {
                return Get<EchoPlaylist>(url);
            }
            catch (Exception exception)
            {
                _logService.AddExceptionFull(string.Format("EchonestService.GetPlaylistByGenre({0})", mergeGenres),
                    exception);
            }

            return EchoPlaylist.Empty();
        }

        /// <summary>
        /// Get similar genres by artist name. Information extract from echonest service.
        /// </summary>
        /// <param name="artistName">Artist name</param>
        /// <param name="count">Genre count</param>
        /// <returns>List of genre names.</returns>
        public List<string> GetSimilarGenres(string artistName, int count = 5)
        {
            try
            {
                var config = _configurationService.GetEchonestConfig();

                var url = string.Format("{0}artist/search?api_key={1}&name={2}&format=xml&results=1", config.BaseUrl,
                    config.AccessToken, artistName);

                // Search artist:
                var response = Get<EchoArtistSearch>(url);
                if (response == null || response.Artists == null || response.Artists.Count == 0)
                    return new List<string>();

                var artist = response.Artists.First();

                // Get similar tags:
                url = string.Format("{0}artist/terms?api_key={1}&id={2}&format=xml", config.BaseUrl, config.AccessToken,
                    artist.Id);

                var termsResponse = Get<EchoArtistTerms>(url);

                if (termsResponse == null)
                    return new List<string>();

                return termsResponse.Terms.Select(t => t.Name).Take(count).ToList();
            }
            catch (Exception exception)
            {
                _logService.AddExceptionFull(string.Format("EchonestService.GetSimilarGenres({0})", artistName),
                    exception);

                return new List<string>();
            }
        }

        private T Get<T>(string url)
        {
            var webClient = new WebClient();
            var content = webClient.DownloadString(url);

            return XmlDeserializeFromString<T>(content);
        }

        #region Utils

        public static T XmlDeserializeFromString<T>(string objectData)
        {
            return (T) XmlDeserializeFromString(objectData, typeof (T));
        }

        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        #endregion
    }
}
