using System;
using Sitemapify.Config;
using Sitemapify.Umbraco;
using Sitemapify.Umbraco.Events;

namespace SitemapifyUmbracoSample.Code
{
    public class MySitemapifyUmbracoContentProvider : SitemapifyUmbracoContentProvider
    {
        protected override Uri GetBaseUrl(Uri baseUri)
        {
            return new Uri("https://github.com/stormid/sitemapify", UriKind.Absolute);
        }
    }

    public class SitemapifyApplicationEventHandler : AbstractSitemapifyApplicationEventHandler
    {
        protected override void ConfigureWith(ISitemapifyConfigurer configure)
        {
            configure.UsingContentProvider(new MySitemapifyUmbracoContentProvider());
        }
    }
}