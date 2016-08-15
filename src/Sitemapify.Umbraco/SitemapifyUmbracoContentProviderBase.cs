using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitemapify.Models;
using Sitemapify.Providers;
using Sitemapify.Umbraco.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace Sitemapify.Umbraco
{
    public class SitemapifyUmbracoContentProvider : ISitemapContentProvider
    {
        protected virtual string ExcludedFromSitemapPropertyAlias { get; } = "umbracoNaviHide";
        protected virtual string ExcludedChildrenFromSitemapPropertyAlias { get; } = "sitemapifyExcludeChildren";

        public SitemapifyUmbracoContentProvider(string excludedFromSitemapPropertyAlias = "umbracoNaviHide",
            string excludedChildrenFromSitemapPropertyAlias = "sitemapifyExcludeChildren")
        {
            ExcludedFromSitemapPropertyAlias = excludedFromSitemapPropertyAlias;
            ExcludedChildrenFromSitemapPropertyAlias = excludedChildrenFromSitemapPropertyAlias;
        }

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
                return home.AllChildren(c => !c.HideFromSitemap(ExcludedFromSitemapPropertyAlias), c => !c.HideChildrenFromSitemap(ExcludedChildrenFromSitemapPropertyAlias)).Select(CreateSitemapUrlForContent);
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