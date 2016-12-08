using Sitemapify.Config;
using Umbraco.Core;

namespace Sitemapify.Umbraco.Events
{
    public abstract class AbstractSitemapifyApplicationEventHandler : ApplicationEventHandler
    {
        protected sealed override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationInitialized(umbracoApplication, applicationContext);
        }

        protected sealed override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        protected sealed override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Configure.With(ConfigureWith);
        }

        protected virtual void ConfigureWith(ISitemapifyConfigurer configure)
        {
            configure.UsingContentProvider(new SitemapifyUmbracoContentProvider());
        }

        protected sealed override bool ExecuteWhenApplicationNotConfigured { get; } = false;
        protected sealed override bool ExecuteWhenDatabaseNotConfigured { get; } = false;
    }
}
