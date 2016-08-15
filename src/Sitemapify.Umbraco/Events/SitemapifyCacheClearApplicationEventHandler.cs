using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace Sitemapify.Umbraco.Events
{
    public class SitemapifyCacheClearApplicationEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Published += (sender, args) => InvalidateCache();
            ContentService.Deleted += (sender, args) => InvalidateCache();
            ContentService.Moved += (sender, args) => InvalidateCache();
            ContentService.Copied += (sender, args) => InvalidateCache();
            ContentService.RolledBack += (sender, args) => InvalidateCache();
        }

        private static void InvalidateCache()
        {
            SitemapifyHttpHandler.ResetCache = true;
            LogHelper.Info<SitemapifyCacheClearApplicationEventHandler>("[Sitemapify] invalidated cache, sitemap will be re-generated on next request");
        }


        protected override bool ExecuteWhenApplicationNotConfigured { get; } = false;
        protected override bool ExecuteWhenDatabaseNotConfigured { get; } = false;
    }
}