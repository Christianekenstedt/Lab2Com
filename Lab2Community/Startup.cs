using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lab2Community.Startup))]
namespace Lab2Community
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
