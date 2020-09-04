using Sitemapify.Config;
using Umbraco.Core.Composing;

namespace Sitemapify.Umbraco.Events
{
    public abstract class AbstractSitemapifyComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            Configure.With(ConfigureWith);
        }

        protected virtual void ConfigureWith(ISitemapifyConfigurer configure)
        {
            configure.UsingContentProvider(new SitemapifyUmbracoContentProvider());
        }
    }
}
