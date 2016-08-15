using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;

namespace Sitemapify.Umbraco.Events
{
    public class SitemapifyApplicationEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Configure.With(config => config.UsingContentProvider(new SitemapifyUmbracoContentProvider()));
        }

        protected override bool ExecuteWhenApplicationNotConfigured { get; } = false;
        protected override bool ExecuteWhenDatabaseNotConfigured { get; } = false;
    }
}
