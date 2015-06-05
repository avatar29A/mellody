using DotLastFm.Api;
using DotLastFm.Api.Rest;
using DotLastFm.Models;
using DotLastFm.Models.Wrappers;

namespace Hqub.Mellody.Music.Services.LastfmExt
{
    public class ExtendArtistApi : ArtistApi
    {
        public ExtendArtistApi(ILastFmApi api)
            : base(api)
        {
            
        }

        public ArtistWithDetails GetInfo(string artist, bool autocorrect, string lang, string username)
        {
            var call = Rest.Method("artist.getInfo")
                .AddParam("artist", artist)
                .AddParam("lang", lang)
                .AddParam("autocorrect", autocorrect ? "1" : "0");

            if (username != null)
            {
                call.AddParam("username", username);
            }

            var wrapper = call.Execute<ArtistWithDetailsWrapper>();

            if (wrapper != null)
            {
                return wrapper.Artist;
            }

            return null;
        }
    }
}
