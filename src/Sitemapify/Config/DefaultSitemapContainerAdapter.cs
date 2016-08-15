using System;
using System.Collections.Generic;
using Sitemapify.Builders;
using Sitemapify.Builders.Impl;
using Sitemapify.Providers;
using Sitemapify.Providers.Impl;

namespace Sitemapify.Config
{
    public class DefaultSitemapContainerAdapter : ISitemapContainerAdapter, ISitemapifyConfigurer
    {
        private static readonly IDictionary<Type, object> Registrations = new Dictionary<Type, object>()
        {
            { typeof(ISitemapContentProvider), new EmptySitemapContentProvider() },
            { typeof(ISitemapDocumentBuilder), new DefaultSitemapDocumentBuilder() },
            { typeof(ISitemapCacheProvider), new DefaultSitemapCacheProvider() },
        };
        
        public T Resolve<T>() where T : class
        {
            if (Registrations.ContainsKey(typeof (T)))
            {
                return Registrations[typeof (T)] as T;
            }
            return null;
        }

        public ISitemapifyConfigurer UsingContentProvider(ISitemapContentProvider contentProvider)
        {
            Registrations[typeof (ISitemapContentProvider)] = contentProvider;
            return this;
        }

        public ISitemapifyConfigurer UsingCacheProvider(ISitemapCacheProvider cacheProvider)
        {
            Registrations[typeof(ISitemapCacheProvider)] = cacheProvider;
            return this;
        }

        public ISitemapifyConfigurer UsingDocumentBuilder(ISitemapDocumentBuilder documentBuilder)
        {
            Registrations[typeof(ISitemapDocumentBuilder)] = documentBuilder;
            return this;
        }
    }
}