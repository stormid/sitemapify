using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sitemapify.Models;
using Sitemapify.Providers.Impl;
using Sitemapify.Umbraco.Config;
using Sitemapify.Umbraco.Extensions;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Sitemapify.Umbraco
{
    public class SitemapifyUmbracoContentProvider : AbstractSitemapContentProvider<IPublishedContent>
    {
        private readonly ISitemapifyUmbracoContentProviderSettings _settings;
        private readonly IUmbracoContextFactory _contextFactory;

        protected SitemapifyUmbracoContentProvider(ISitemapifyUmbracoContentProviderSettings settings, IUmbracoContextFactory contextFactory)
        {
            _settings = settings;
            _contextFactory = contextFactory;
        }

        public SitemapifyUmbracoContentProvider() : this(SitemapifyUmbracoContentProviderSettings.Current, DependencyResolver.Current.GetService<IUmbracoContextFactory>()) {}


        protected UmbracoContext GetUmbracoContext()
        {
            return _contextFactory?.EnsureUmbracoContext()?.UmbracoContext;
        }

        protected override SitemapUrl CreateSitemapUrl(IPublishedContent entry, Uri baseUri)
        {
            var uri = entry.Url(mode: UrlMode.Absolute);
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                uri = $"{baseUri.ToString().TrimEnd('/')}{entry.Url}";
            }

            var absoluteUri = uri.TrimEnd('/');
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
            return context.Content.GetByRoute("/");
        }

        public override bool Cacheable { get; } = true;

        public override DateTime CacheUntil { get; } = DateTime.UtcNow.AddHours(1);
    }
}