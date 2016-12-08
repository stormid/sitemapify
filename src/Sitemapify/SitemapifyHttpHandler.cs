using System;
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
        public static bool ResetCache { get; set; } = false;

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
            var document = GetSitemapContent(context.Request);
            document.Save(context.Response.Output, SaveOptions.OmitDuplicateNamespaces);
            if (_contentProvider.Cacheable)
            {
                context.Response.Cache.SetExpires(_contentProvider.CacheUntil);
                context.Response.Cache.SetCacheability(HttpCacheability.Server);
            }
        }

        private XDocument GetSitemapContent(HttpRequestBase contextRequest)
        {
            if (!_sitemapCacheProvider.IsCached || ResetCache)
            {
                var baseUrl = contextRequest.Url?.GetLeftPart(UriPartial.Authority);
                if (!string.IsNullOrWhiteSpace(baseUrl) && Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
                {
                    var authorityUri = new Uri(baseUrl, UriKind.Absolute);
                    var document = _documentBuilder.BuildSitemapXmlDocument(_contentProvider.GetSitemapUrls(authorityUri));
                    if (_contentProvider.Cacheable)
                    {
                        _sitemapCacheProvider.Add(document, _contentProvider.CacheUntil);
                        ResetCache = false;
                    }
                    return document;
                }
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