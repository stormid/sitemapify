using System;
using System.Collections.Generic;

namespace Sitemapify
{
    public interface ISitemapContentProvider
    {
        IEnumerable<SitemapUrl> GetSitemapUrls();
        bool Cacheable { get; }
        DateTime? CacheUntil { get; }
    }
}