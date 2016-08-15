namespace Sitemapify.Umbraco.Config
{
    public static class SitemapifyUmbracoContentProviderSettings
    {
        private static ISitemapifyUmbracoContentProviderSettings _current = new FromConfigSitemapifyUmbracoContentProviderSettings();

        public static ISitemapifyUmbracoContentProviderSettings Current => _current;

        public static void Set(ISitemapifyUmbracoContentProviderSettings settings)
        {
            _current = settings;
        }
    }
}