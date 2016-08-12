namespace Sitemapify
{
    public class EmptySitemapContentProviderFactory : ISitemapContentProviderFactory
    {
        public ISitemapContentProvider GetProvider()
        {
            return new EmptySitemapContentProvider();
        }
    }
}