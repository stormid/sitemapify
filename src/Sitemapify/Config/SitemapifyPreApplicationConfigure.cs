using System.Web.Routing;

namespace Sitemapify.Config
{
    public static class SitemapifyPreApplicationConfigure
    {
        public static void Initialise()
        {
            RouteTable.Routes.Ignore("sitemap.xml");
        }
    }
}