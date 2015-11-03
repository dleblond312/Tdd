using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tdd.Startup))]
namespace Tdd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
