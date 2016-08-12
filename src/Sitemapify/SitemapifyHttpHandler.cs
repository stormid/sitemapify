using System;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using Sitemapify.Config;

namespace Sitemapify
{
    public sealed class SitemapifyHttpHandler : IHttpHandler, IRouteHandler
    {
        private readonly ISitemapContentProviderFactory _factory;
        private readonly ISitemapDocumentBuilder _documentBuilder;

        private SitemapifyHttpHandler(ISitemapContentProviderFactory factory, ISitemapDocumentBuilder documentBuilder)
        {
            _factory = factory;
            _documentBuilder = documentBuilder;
        }

        public SitemapifyHttpHandler() : this(SitemapifyConfigure.ContentProvider, SitemapifyConfigure.DocumentBuilder) { }

        public void ProcessRequest(HttpContextBase context)
        {
            context.Response.ContentType = "text/xml";
            context.Response.Buffer = true;
            context.Response.BufferOutput = true;
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Output.Write(GetSitemapContent());
        }

        private string GetSitemapContent()
        {
            var cache = MemoryCache.Default;

            var sitemapCacheContent = cache.Get(nameof(SitemapifyHttpHandler))?.ToString();
            if (!string.IsNullOrWhiteSpace(sitemapCacheContent))
            {
                return sitemapCacheContent;
            }
            var provider = _factory.GetProvider();
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            _documentBuilder.BuildSitemapXmlStream(provider.GetSitemapUrls(), writer);
            if (provider.Cacheable)
            {
                cache.Add(nameof(SitemapifyHttpHandler), sb.ToString(), DateTimeOffset.UtcNow.AddHours(1));
            }
            return sb.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        public bool IsReusable { get; } = true;

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }
    }
}