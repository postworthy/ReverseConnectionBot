using Owin;
using Microsoft.Owin;

namespace ReverseConnectionBot
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}