using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Harbour.RedisSessionStateStore.Mvc.Startup))]
namespace Harbour.RedisSessionStateStore.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
