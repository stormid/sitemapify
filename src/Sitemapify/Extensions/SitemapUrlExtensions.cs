using System.Linq;
using System.Xml.Linq;

namespace Sitemapify.Extensions
{
    internal static class SitemapUrlExtensions
    {
        public static XElement ToXElement(this SitemapUrl sitemapUrl, string ns = SitemapUrl.SitemapNs)
        {
            var parts = new object[]
            {
                new XElement(XName.Get(nameof(SitemapUrl.Loc).ToLowerInvariant(),ns), sitemapUrl.Loc),
                sitemapUrl.Lastmod.HasValue ? new XElement(XName.Get(nameof(SitemapUrl.Lastmod).ToLowerInvariant(), ns), sitemapUrl.Lastmod.Value.ToString("O")) : null,
                sitemapUrl.ChangeFreq.HasValue ? new XElement(XName.Get(nameof(SitemapUrl.ChangeFreq).ToLowerInvariant(), ns), sitemapUrl.ChangeFreq.Value.ToString().ToLowerInvariant()) : null,
                sitemapUrl.Priority.HasValue ? new XElement(XName.Get(nameof(SitemapUrl.Priority).ToLowerInvariant(), ns), sitemapUrl.Priority.Value) : null
            }.ToList();

            return new XElement(XName.Get("url", ns), parts);
        }
    }
}