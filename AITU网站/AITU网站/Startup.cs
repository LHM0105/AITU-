using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AITU网站.Startup))]
namespace AITU网站
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
