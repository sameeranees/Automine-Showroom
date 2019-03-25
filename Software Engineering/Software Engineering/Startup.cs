using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Software_Engineering.Startup))]
namespace Software_Engineering
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
