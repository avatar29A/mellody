using AutoMapper;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;
using Hqub.Mellody.Web.MapperConverters;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hqub.Mellody.Web.Startup))]
namespace Hqub.Mellody.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMapping();
        }

        private void ConfigureMapping()
        {
            Mapper.CreateMap<Track, TrackDTO>();
            Mapper.CreateMap<Playlist, PlaylistDTO>();
            Mapper.CreateMap<Station, StationDTO>();
        }
    }
}
