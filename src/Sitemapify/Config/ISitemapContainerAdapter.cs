namespace Sitemapify.Config
{
    public interface ISitemapContainerAdapter
    {
        T Resolve<T>() where T : class;
    }
}