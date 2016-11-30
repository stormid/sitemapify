using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using FluentAssertions;
using Moq;
using Sitemapify.Config;
using Sitemapify.Models;
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

    public class SitemapifyRecursiveContentLookupTests
    {
        public class ContentModelStub
        {
            public ContentModelStub(ContentModelStub parent)
            {
                Parent = parent;
            }

            public string Id { get; set; } = Guid.NewGuid().ToString("N");

            public ContentModelStub Parent { get; }

            public bool HideFromSitemap { get; set; }

            public bool HideChildrenFromSitemap { get; set; }

            public IEnumerable<ContentModelStub> ChildNodes { get; set; }

            public IEnumerable<ContentModelStub> Children(Func<ContentModelStub, bool> where = null)
            {
                if (where == null) return Enumerable.Empty<ContentModelStub>();
                return ChildNodes?.Where(where) ?? Enumerable.Empty<ContentModelStub>();
            }

            public override string ToString()
            {
                return $"({Parent}-{HideFromSitemap}-{HideChildrenFromSitemap})";
            }
        }

        public IEnumerable<ContentModelStub> Matching(ContentModelStub node, Func<ContentModelStub, bool> predicate = null, Func<ContentModelStub, bool> includeChildren = null)
        {
            if (predicate?.Invoke(node) ?? true)
            {
                yield return node;
            }

            if (includeChildren?.Invoke(node) ?? true)
            {
                foreach (var child in node.Children(predicate))
                {
                    foreach (var grandChild in Matching(child, predicate, includeChildren))
                    {
                        yield return grandChild;
                    }
                }
            }
        }

        [Fact]
        public void TopNodeIsHidden()
        {
            // arrange
            var top = new ContentModelStub(null);
            top.HideFromSitemap = true;

            var result = Matching(top, c => !c.HideFromSitemap, c => !c.HideChildrenFromSitemap).ToList();
            result.Count.Should().Be(0);
        }

        [Fact]
        public void TopNodeIsVisible()
        {
            // arrange
            var top = new ContentModelStub(null);

            var result = Matching(top, c => !c.HideFromSitemap, c => !c.HideChildrenFromSitemap).ToList();
            result.Count.Should().Be(1);
        }

        [Fact]
        public void TopNodeHasChildren()
        {
            // arrange
            var top = new ContentModelStub(null);
            top.ChildNodes = new[]
            {
                new ContentModelStub(top),
                new ContentModelStub(top),
                new ContentModelStub(top)
            };

            var result = Matching(top, c => !c.HideFromSitemap, c => !c.HideChildrenFromSitemap).ToList();
            result.Count.Should().Be(4);
        }

        [Fact]
        public void TopNodeHasHiddenChildren()
        {
            // arrange
            var top = new ContentModelStub(null);
            top.HideChildrenFromSitemap = true;
            top.ChildNodes = new[]
            {
                new ContentModelStub(top),
                new ContentModelStub(top),
                new ContentModelStub(top)
            };

            var result = Matching(top, c => !c.HideFromSitemap, c => !c.HideChildrenFromSitemap).ToList();
            result.Count.Should().Be(1);
        }

        [Fact]
        public void TopNodeTree()
        {
            // arrange
            var top = new ContentModelStub(null);

            var child = new ContentModelStub(top) {HideChildrenFromSitemap = true};
            child.ChildNodes = new[]
            {
                new ContentModelStub(child)
            };

            top.ChildNodes = new[]
            {
                child
            };

            var result = Matching(top, c => !c.HideFromSitemap, c => !c.HideChildrenFromSitemap).ToList();
            result.Count.Should().Be(2);
        }
    }
}
