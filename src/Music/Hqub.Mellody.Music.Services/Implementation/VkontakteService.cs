using System.Collections.Generic;
using System.Linq;
using Hqub.Mellody.Music.Services.Interfaces;
using Hqub.Mellody.Poco;
using Hqub.Mellowave.Vkontakte.API.Factories;

namespace Hqub.Mellody.Music.Services.Implementation
{
    public class VkontakteService : IVkontakteService
    {
        private readonly IConfigurationService _config;
        private readonly ILogService _logService;
        private readonly ApiFactory _api;

        public VkontakteService(IConfigurationService config, ILogService logService)
        {
            _config = config;
            _logService = logService;
            _api = ApiFactory.Instance(GetToken());
        }

        public List<SearchTrackDTO> SearchTracks(string query)
        {
            var audioApi = _api.GetAudioProduct();
            var response = audioApi.Search(query, 10);

            if(response == null)
                return new List<SearchTrackDTO>();

            return (from track in response.Tracks
                select
                    new SearchTrackDTO(track.ToString(), track.ComplexId, track.Url,
                        RankHelper.Calc(query, track.ToString()))).ToList();
        }

        private string GetToken()
        {
            var authConfigure = _config.GetVkontakteAuthConfig();

            var r = Utilities.PythonInvoker.Execute(authConfigure.PythonPath, new List<string>
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
