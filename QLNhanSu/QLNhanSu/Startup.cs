using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLNhanSu.Startup))]
namespace QLNhanSu
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
