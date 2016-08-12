using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using FluentAssertions;
using Moq;
using Sitemapify.Config;
using Xunit;

namespace Sitemapify.Tests
{
    
    public class SitemapContentProviderTests
    {
        public SitemapContentProviderTests()
        {
            MemoryCache.Default.Trim(100);
        }

        [Fact]
        public void When()
        {
            var sb = new StringBuilder();
            var responseWriter = new StringWriter(sb);

            var httpContext = new Mock<HttpContextBase>(MockBehavior.Loose);
            httpContext.Setup(s => s.Request).Returns(() => new HttpRequestWrapper(new HttpRequest("sitemap.xml", "sitemap.xml", "")));
            httpContext.Setup(s => s.Response).Returns(() => new HttpResponseWrapper(new HttpResponse(responseWriter)));

            var handler = new SitemapifyHttpHandler();
            handler.ProcessRequest(httpContext.Object);

            var document = XDocument.Parse(sb.ToString());
            (from urls in
                    document.Descendants(XName.Get("urlset", SitemapUrl.SitemapNs))
                        .Elements(XName.Get("url", SitemapUrl.SitemapNs))
                select urls).Count().Should().Be(1);
        }
    }

    public class SitemapUrlTests
    {
    }
}
