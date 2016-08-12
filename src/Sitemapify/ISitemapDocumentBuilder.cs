using System;
using System.Collections.Generic;
using System.IO;

namespace Sitemapify
{
    public interface ISitemapDocumentBuilder
    {
        void BuildSitemapXmlStream(IEnumerable<SitemapUrl> sitemapUrls, TextWriter writer);
    }
}