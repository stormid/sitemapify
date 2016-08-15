using System;
using System.Runtime.Caching;
using System.Xml.Linq;

namespace Sitemapify.Providers.Impl
{
    public class DefaultSitemapCacheProvider : ISitemapCacheProvider
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        public void Add(XDocument sitemapContent, DateTimeOffset? expires)
        {
            _cache.Add(nameof(SitemapifyHttpHandler), sitemapContent, expires.GetValueOrDefault(ObjectCache.InfiniteAbsoluteExpiration));
        }

        public bool IsCached => _cache.Contains(nameof(SitemapifyHttpHandler));

        public XDocument Get()
        {
            return _cache.Get(nameof(SitemapifyHttpHandler)) as XDocument;
        }
    }
}