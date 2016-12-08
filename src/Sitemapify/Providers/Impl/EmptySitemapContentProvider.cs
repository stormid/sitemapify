using System;
using System.Collections.Generic;
using Sitemapify.Models;

namespace Sitemapify.Providers.Impl
{
    public class EmptySitemapContentProvider : ISitemapContentProvider
    {
        public virtual IEnumerable<SitemapUrl> GetSitemapUrls()
        {
            yield return SitemapUrl.Create("/");
        }

        public virtual bool Cacheable { get; } = true;
        public virtual DateTime CacheUntil { get; } = DateTime.UtcNow.AddHours(1);
    }
}