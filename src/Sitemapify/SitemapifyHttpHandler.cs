using System.Text;
using System.Web;
using System.Web.Routing;
using System.Xml.Linq;
using Sitemapify.Builders;
using Sitemapify.Providers;

namespace Sitemapify
{
    public sealed class SitemapifyHttpHandler : IHttpHandler, IRouteHandler
    {
        private readonly ISitemapContentProvider _contentProvider;
        private readonly ISitemapDocumentBuilder _documentBuilder;
        private readonly ISitemapCacheProvider _sitemapCacheProvider;

        private SitemapifyHttpHandler(ISitemapContentProvider contentProvider, ISitemapDocumentBuilder documentBuilder, ISitemapCacheProvider sitemapCacheProvider)
        {
            _contentProvider = contentProvider;
            _documentBuilder = documentBuilder;
            _sitemapCacheProvider = sitemapCacheProvider;
        }

        public SitemapifyHttpHandler() : this(Configure.ContentProvider, Configure.DocumentBuilder, Configure.CacheProvider) { }

        public void ProcessRequest(HttpContextBase context)
        {
            context.Response.ContentType = "text/xml";
            context.Response.Buffer = true;
            context.Response.BufferOutput = true;
            context.Response.ContentEncoding = Encoding.UTF8;
            var document = GetSitemapContent();
            document.Save(context.Response.Output, SaveOptions.OmitDuplicateNamespaces);
            if (_contentProvider.Cacheable)
            {
                context.Response.Cache.SetExpires(_contentProvider.CacheUntil);
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
            }
        }

        private XDocument GetSitemapContent()
        {
            if (!_sitemapCacheProvider.IsCached)
            {
                var document = _documentBuilder.BuildSitemapXmlDocument(_contentProvider.GetSitemapUrls());
                if (_contentProvider.Cacheable)
                {
                    _sitemapCacheProvider.Add(document, _contentProvider.CacheUntil);
                }
                return document;
            }
            return _sitemapCacheProvider.Get();
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