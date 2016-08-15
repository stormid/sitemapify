namespace Sitemapify.Umbraco.Config
{
    public class FromConfigSitemapifyUmbracoContentProviderSettings : ISitemapifyUmbracoContentProviderSettings
    {
        private const string Prefix = "Sitemapify.Umbraco:";

        public string ExcludedFromSitemapPropertyAlias { get; } =
            nameof(ExcludedFromSitemapPropertyAlias).FromAppSettingsWithPrefix(Prefix, "umbracoNaviHide");

        public string ExcludedChildrenFromSitemapPropertyAlias { get; } =
            nameof(ExcludedChildrenFromSitemapPropertyAlias)
                .FromAppSettingsWithPrefix(Prefix, "sitemapifyExcludeChildren");
    }
}