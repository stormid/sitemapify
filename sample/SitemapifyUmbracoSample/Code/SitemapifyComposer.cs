using System;
using Sitemapify;
using Sitemapify.Umbraco;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace SitemapifyUmbracoSample.Code
{
    public class MySitemapifyUmbracoContentProvider : SitemapifyUmbracoContentProvider
    {
        protected override Uri GetBaseUrl(Uri baseUri)
        {
            return new Uri("https://github.com/stormid/sitemapify", UriKind.Absolute);
        }
    }

    public class SitemapifyComponent : IComponent
    {
        public void Initialize()
        {
            Configure.With(config => config.UsingContentProvider(new SitemapifyUmbracoContentProvider()));
        }

        public void Terminate() { }
    }

    public class SitemapifyComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<SitemapifyComponent>();
        }
    }
}