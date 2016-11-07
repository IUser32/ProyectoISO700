using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProyectoISO700.Startup))]
namespace ProyectoISO700
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
