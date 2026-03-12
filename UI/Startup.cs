using Business.Middlewares;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UI.Startup))]
namespace UI
{
    public  class Startup:Auth
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
