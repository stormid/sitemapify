using System;
using Sitemapify.Builders;
using Sitemapify.Config;
using Sitemapify.Providers;

namespace Sitemapify
{
    public static class Configure
    {
        private static ISitemapContainerAdapter _containerAdapter;
        private static readonly DefaultSitemapContainerAdapter DefaultContainerAdapter = new DefaultSitemapContainerAdapter();

        static Configure()
        {
            _containerAdapter = DefaultContainerAdapter;
        }

        internal static ISitemapContentProvider ContentProvider
            => _containerAdapter.Resolve<ISitemapContentProvider>() ?? DefaultContainerAdapter.Resolve<ISitemapContentProvider>();

        internal static ISitemapDocumentBuilder DocumentBuilder => _containerAdapter.Resolve<ISitemapDocumentBuilder>() ?? DefaultContainerAdapter.Resolve<ISitemapDocumentBuilder>();

        internal static ISitemapCacheProvider CacheProvider => _containerAdapter.Resolve<ISitemapCacheProvider>() ?? DefaultContainerAdapter.Resolve<ISitemapCacheProvider>();

        public static bool InvalidateCache { get; set; } = false;

        public static void With(ISitemapContainerAdapter containerAdapter)
        {
            _containerAdapter = containerAdapter;
        }

        public static void With(Action<ISitemapifyConfigurer> configurer)
        {
            configurer(DefaultContainerAdapter);
        }
    }
}