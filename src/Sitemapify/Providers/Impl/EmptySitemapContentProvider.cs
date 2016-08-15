using System;
using System.Collections.Generic;
using Sitemapify.Models;

namespace Sitemapify.Providers.Impl
{
    public class EmptySitemapContentProvider : ISitemapContentProvider
    {
        public IEnumerable<SitemapUrl> GetSitemapUrls()
        {
            yield return SitemapUrl.Create("/");
        }

        public bool Cacheable { get; } = true;
        public DateTime CacheUntil { get; } = DateTime.UtcNow.AddHours(1);
    }
}