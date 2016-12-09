using System;
using System.Collections.Generic;
using System.Linq;
using Sitemapify.Models;

namespace Sitemapify.Providers.Impl
{
    public class EmptySitemapContentProvider : AbstractSitemapContentProvider<object>
    {
        protected override SitemapUrl CreateSitemapUrl(object entry, Uri baseUri)
        {
            var ub = new UriBuilder(baseUri)
            {
                Path = "/"
                
            };
            return SitemapUrl.Create(ub.Uri.ToString(), changeFreq: SitemapChangeFrequency.Never);
        }

        protected override IEnumerable<object> GetSitemapEntries()
        {
            return Enumerable.Empty<object>();
        }
    }
}