using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Sitemapify;
using Sitemapify.Providers;
using Sitemapify.Providers.Impl;
using Sitemapify.Umbraco;
using Umbraco.Core;
using Umbraco.Core.Persistence.Migrations.Upgrades.TargetVersionSix;

namespace SitemapifyUmbracoSample.Code
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