using Castle.Core.Configuration;
using Castle.MicroKernel;
using Sitemapify.Config;

namespace Sitemapify.CastleWindsor
{
    public class CastleWindsorSitemapifyContainerAdapter : ISitemapContainerAdapter
    {
        private readonly IKernel _kernel;
        public CastleWindsorSitemapifyContainerAdapter(IKernel kernel)
        {
            _kernel = kernel;
        }

        public T Resolve<T>() where T : class
        {
            if (_kernel.HasComponent(typeof (T))) return _kernel.Resolve<T>();
            return null;
        }
    }
}
