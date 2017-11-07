using Microsoft.Owin;
using Owin;
using UCS.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace UCS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}