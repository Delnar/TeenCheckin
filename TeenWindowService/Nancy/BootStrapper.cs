using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;
using Nancy.Session;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using System.Web.Routing;
using Nancy.TinyIoc;

namespace TeenWindowService.Nancy
{
    public class BootStrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Clear();
            nancyConventions.StaticContentsConventions.Add
            (StaticContentConventionBuilder.AddDirectory("", "/SPA"));
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            ConfigManager mgr;
            mgr = new ConfigManager();
            mgr.LoadConfig();
            container.Register(mgr);
        }
    }
}
