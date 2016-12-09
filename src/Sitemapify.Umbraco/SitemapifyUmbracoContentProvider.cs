using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitemapify.Models;
using Sitemapify.Providers.Impl;
using Sitemapify.Umbraco.Config;
using Sitemapify.Umbraco.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace Sitemapify.Umbraco
{
    public class SitemapifyUmbracoContentProvider : AbstractSitemapContentProvider<IPublishedContent>
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

        protected override SitemapUrl CreateSitemapUrl(IPublishedContent entry, Uri baseUri)
        {
            var uri = entry.UrlAbsolute();
            if(!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                uri = $"{baseUri}{entry.Url()}";
            }
            
            var absoluteUri = uri.TrimEnd("/");
            return SitemapUrl.Create(absoluteUri, entry.UpdateDate);
        }
        
        protected sealed override IEnumerable<IPublishedContent> GetSitemapEntries()
        {
            var ctx = GetUmbracoContext();
            if (ctx != null)
            {
                var root = FromContent(ctx);
                if (root.ItemType == PublishedItemType.Content)
                {
                    return root
                        .DescendantSitemapNodes(_settings.ExcludedFromSitemapPropertyAlias, _settings.ExcludedChildrenFromSitemapPropertyAlias)
                        .Where(node => node.ItemType == PublishedItemType.Content);
                }
            }
            return Enumerable.Empty<IPublishedContent>();
        }

        /// <summary>
        /// The content node from which the sitemap should start
        /// </summary>
        /// <param name="context">The Umbraco context</param>
        /// <returns>A content node, media nodes are not supported</returns>
        protected virtual IPublishedContent FromContent(UmbracoContext context)
        {
            return context.ContentCache.GetByRoute("/");
        }

        public override bool Cacheable { get; } = true;

        public override DateTime CacheUntil { get; } = DateTime.UtcNow.AddHours(1);
    }
}