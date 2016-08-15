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
            Remove();
            _cache.Add(nameof(SitemapifyHttpHandler), sitemapContent, expires.GetValueOrDefault(ObjectCache.InfiniteAbsoluteExpiration));
        }

        public virtual bool IsCached => _cache.Contains(nameof(SitemapifyHttpHandler));

        public void Remove()
        {
            _cache.Remove(nameof(SitemapifyHttpHandler));
        }

        public XDocument Get()
        {
            return _cache.Get(nameof(SitemapifyHttpHandler)) as XDocument;
        }
    }
}