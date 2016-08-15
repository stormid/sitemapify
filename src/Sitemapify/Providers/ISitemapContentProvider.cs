using System;
using System.Collections.Generic;
using Sitemapify.Models;

namespace Sitemapify.Providers
{
    public interface ISitemapContentProvider
    {
        IEnumerable<SitemapUrl> GetSitemapUrls();
        bool Cacheable { get; }
        DateTime CacheUntil { get; }
    }
}