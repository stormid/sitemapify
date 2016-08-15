using System;
using System.Collections.Generic;
using Sitemapify.Builders;
using Sitemapify.Builders.Impl;
using Sitemapify.Providers;
using Sitemapify.Providers.Impl;

namespace Sitemapify.Config
{
    public class DefaultSitemapContainerAdapter : ISitemapContainerAdapter
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
    }
}