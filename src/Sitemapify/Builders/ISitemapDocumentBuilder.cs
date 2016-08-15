using System.Collections.Generic;
using System.Xml.Linq;
using Sitemapify.Models;

namespace Sitemapify.Builders
{
    public interface ISitemapDocumentBuilder
    {
        XDocument BuildSitemapXmlDocument(IEnumerable<SitemapUrl> sitemapUrls);
    }
}