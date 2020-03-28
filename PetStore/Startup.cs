using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PetStore.Startup))]
namespace PetStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
