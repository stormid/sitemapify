using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sitemapify.Builders;
using Sitemapify.Builders.Impl;
using Sitemapify.Providers;
using Sitemapify.Providers.Impl;

namespace Sitemapify.CastleWindsor
{
    public class SitemapifyWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ISitemapContentProvider>().ImplementedBy<EmptySitemapContentProvider>().IsFallback().LifestylePerWebRequest(),
                Component.For<ISitemapCacheProvider>().ImplementedBy<DefaultSitemapCacheProvider>().IsFallback().LifestylePerWebRequest(),
                Component.For<ISitemapDocumentBuilder>().ImplementedBy<DefaultSitemapDocumentBuilder>().IsFallback().LifestylePerWebRequest()
                );

            Configure.With(new CastleWindsorSitemapifyContainerAdapter(container.Kernel));
        }
    }
}