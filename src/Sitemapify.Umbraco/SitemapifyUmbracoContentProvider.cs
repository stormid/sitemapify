using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Sitemapify.Models;
using Sitemapify.Providers;
using Sitemapify.Providers.Impl;
using Sitemapify.Umbraco.Config;
using Sitemapify.Umbraco.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace Sitemapify.Umbraco
{
    public class SitemapifyUmbracoContentProvider : EmptySitemapContentProvider
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

        public sealed override IEnumerable<SitemapUrl> GetSitemapUrls(Uri baseUrl)
        {
            var ctx = GetUmbracoContext();
            if (ctx != null)
            {
                var root = FromContent(ctx);
                if (root.ItemType != PublishedItemType.Content)
                {
                    return Enumerable.Empty<SitemapUrl>();
                }

                var overrideBaseUrl = GetBaseUrl(baseUrl);
                if (overrideBaseUrl.IsAbsoluteUri)
                {
                    return root
                        .DescendantSitemapNodes(_settings.ExcludedFromSitemapPropertyAlias, _settings.ExcludedChildrenFromSitemapPropertyAlias)
                        .Where(node => node.ItemType == PublishedItemType.Content)
                        .Select(content => CreateSitemapUrlForContent(content, overrideBaseUrl));
                }
            }
            return Enumerable.Empty<SitemapUrl>();
        }

        /// <summary>
        /// Must return an absolute uri, otherwise the sitemap will be returned blank
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        protected virtual Uri GetBaseUrl(Uri baseUri)
        {
            return baseUri;
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

        private static SitemapUrl CreateSitemapUrlForContent(IPublishedContent content, Uri authorityUri)
        {
            var ub = new UriBuilder(authorityUri)
            {
                Path = content.Url()
            };
            var absoluteUri = ub.Uri.ToString().TrimEnd("/");
            return SitemapUrl.Create(absoluteUri, content.UpdateDate);

            //var absoluteUri = content.UrlAbsolute();
            //if (!Uri.IsWellFormedUriString(absoluteUri, UriKind.Absolute))
            //{
            //    var ub = new UriBuilder(authorityUri)
            //    {
            //        Path = content.Url()
            //    };
            //    absoluteUri = ub.ToString().TrimEnd("/");
            //}
            //return SitemapUrl.Create(absoluteUri, content.UpdateDate);
        }
    }
}