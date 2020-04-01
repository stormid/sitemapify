using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Services.Implement;

namespace Sitemapify.Umbraco.Events
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SitemapifyCacheClearComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<SitemapifyCacheClearComponent>();
        }
    }

    public class SitemapifyCacheClearComponent : IComponent
    {
        private readonly ILogger _logger;

        public SitemapifyCacheClearComponent(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            ContentService.Published += (sender, args) => InvalidateCache();
            ContentService.Deleted += (sender, args) => InvalidateCache();
            ContentService.Moved += (sender, args) => InvalidateCache();
            ContentService.Copied += (sender, args) => InvalidateCache();
            ContentService.RolledBack += (sender, args) => InvalidateCache();        }

        public void Terminate()
        {
            throw new System.NotImplementedException();
        }

        private void InvalidateCache()
        {
            SitemapifyHttpHandler.ResetCache = true;
            _logger.Info<SitemapifyCacheClearComponent>("[Sitemapify] invalidated cache, sitemap will be re-generated on next request");
        }
    }
}