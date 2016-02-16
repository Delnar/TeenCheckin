using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Hosting.Self;
using System.Net.Sockets;

namespace TeenWindowService.Nancy
{
    public class MainMod : NancyModule
    {
        public MainMod(ConfigManager mgr)
        {
            Get["/config"] = x =>
            {
                return View["views/config.html", mgr.config];
            };
        }
    }
}
