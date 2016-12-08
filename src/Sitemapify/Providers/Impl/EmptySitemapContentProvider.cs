using System;
using System.Collections.Generic;
using Sitemapify.Models;

namespace Sitemapify.Providers.Impl
{
    public class EmptySitemapContentProvider : ISitemapContentProvider
    {
        public virtual IEnumerable<SitemapUrl> GetSitemapUrls(Uri baseUrl)
        {
            var ub = new UriBuilder(baseUrl) {Path = "/"};
            yield return SitemapUrl.Create(ub.ToString());
        }

        public virtual bool Cacheable { get; } = true;
        public virtual DateTime CacheUntil { get; } = DateTime.UtcNow.AddHours(1);
    }
}