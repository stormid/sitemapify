using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Sitemapify.Models;
using Sitemapify.Providers;
using Sitemapify.Umbraco.Config;
using Sitemapify.Umbraco.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace Sitemapify.Umbraco
{
    public class SitemapifyUmbracoContentProvider : ISitemapContentProvider
    {
        private readonly ISitemapifyUmbracoContentProviderSettings _settings;

        protected SitemapifyUmbracoContentProvider(ISitemapifyUmbracoContentProviderSettings settings)
        {
            _settings = settings;
        }

        public SitemapifyUmbracoContentProvider() : this(SitemapifyUmbracoContentProviderSettings.Current) { }

        protected UmbracoContext GetUmbracoContext()
        {
            var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
            return UmbracoContext.EnsureContext(httpContextWrapper, ApplicationContext.Current, new WebSecurity(httpContextWrapper, ApplicationContext.Current));
        }

        public IEnumerable<SitemapUrl> GetSitemapUrls()
        {
            var ctx = GetUmbracoContext();
            if (ctx != null)
            {
                var home = FromContent(ctx);
                return home
                    .DescendantSitemapNodes(_settings.ExcludedFromSitemapPropertyAlias, _settings.ExcludedChildrenFromSitemapPropertyAlias)
                    .Where(node => node.ItemType == PublishedItemType.Content)
                    .Select(CreateSitemapUrlForContent);
            }
            return Enumerable.Empty<SitemapUrl>();
        }

        protected virtual IPublishedContent FromContent(UmbracoContext context)
        {
            return context.ContentCache.GetByRoute("/");
        }

        public virtual bool Cacheable { get; } = true;

        public virtual DateTime CacheUntil { get; } = DateTime.UtcNow.AddHours(1);

        protected virtual SitemapUrl CreateSitemapUrlForContent(IPublishedContent content)
        {
            return SitemapUrl.Create(content.UrlAbsolute(), content.UpdateDate);
        }
    }
}