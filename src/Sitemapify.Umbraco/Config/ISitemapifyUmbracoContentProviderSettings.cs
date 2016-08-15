namespace Sitemapify.Umbraco.Config
{
    public interface ISitemapifyUmbracoContentProviderSettings
    {
        string ExcludedFromSitemapPropertyAlias { get; }
        string ExcludedChildrenFromSitemapPropertyAlias { get; }
    }
}