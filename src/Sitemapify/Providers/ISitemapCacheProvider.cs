using System;
using System.Xml.Linq;

namespace Sitemapify.Providers
{
    public interface ISitemapCacheProvider
    {
        void Add(XDocument sitemapContent, DateTimeOffset? expires);

        bool IsCached { get; }

        XDocument Get();
    }
}