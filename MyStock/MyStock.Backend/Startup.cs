using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyStock.Backend.Startup))]
namespace MyStock.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
