using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VZwars.Startup))]
namespace VZwars
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
