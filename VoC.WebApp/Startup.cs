using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(VoC.WebApp.Startup))]

namespace VoC.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            VoC.DataAccess.DbInit.DbInit.InitDB();
            ConfigureAuth(app);
        }
    }
}
