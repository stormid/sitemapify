using System;
using System.Web.Routing;

namespace Sitemapify.Config
{
    public static class SitemapifyConfigure
    {
        private static ISitemapContainerAdapter _containerAdapter = new DefaultSitemapContainerAdapter();

        internal static ISitemapContentProviderFactory ContentProvider
            => _containerAdapter.Resolve<ISitemapContentProviderFactory>();

        internal static ISitemapDocumentBuilder DocumentBuilder => _containerAdapter.Resolve<ISitemapDocumentBuilder>();

        public static void Initialise()
        {
            RouteTable.Routes.Ignore("sitemap.xml");
        }

        public static void Configure(ISitemapContainerAdapter containerAdapter = null)
        {
            if (_containerAdapter != null) _containerAdapter = containerAdapter;
        }
    }
}