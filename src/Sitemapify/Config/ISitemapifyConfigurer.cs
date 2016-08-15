using Sitemapify.Builders;
using Sitemapify.Models;
using Sitemapify.Providers;

namespace Sitemapify.Config
{
    public interface ISitemapifyConfigurer
    {
        /// <summary>
        /// The content provider is used to get a collection of <see cref="SitemapUrl"/>'s representing the sitemap xml document to generate
        /// </summary>
        /// <param name="contentProvider">The custom content provider</param>
        /// <returns></returns>
        ISitemapifyConfigurer UsingContentProvider(ISitemapContentProvider contentProvider);
        /// <summary>
        /// The cache provider is responsible for retaining the sitemap document once it is generated so as to avoid having to constantly generate it
        /// </summary>
        /// <remarks>The default cache provider uses an in-memory caching mechanism that expires the sitemap 1 hour after initial access</remarks>
        /// <param name="cacheProvider">The custom cache provider</param>
        /// <returns></returns>
        ISitemapifyConfigurer UsingCacheProvider(ISitemapCacheProvider cacheProvider);
        /// <summary>
        /// The document builder is responsible for taking the compiled sitemap urls and generating a valid sitemap schema compatible xml document
        /// </summary>
        /// <remarks>You really shouldn't need to alter this</remarks>
        /// <param name="documentBuilder">The custom document builder</param>
        /// <returns></returns>
        ISitemapifyConfigurer UsingDocumentBuilder(ISitemapDocumentBuilder documentBuilder);
    }
}