using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hqub.Mellody.Web.Startup))]
namespace Hqub.Mellody.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
