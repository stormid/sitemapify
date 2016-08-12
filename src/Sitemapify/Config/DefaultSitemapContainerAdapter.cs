using System;
using System.Collections.Generic;

namespace Sitemapify.Config
{
    public class DefaultSitemapContainerAdapter : ISitemapContainerAdapter
    {
        private static readonly IDictionary<Type, object> Registrations = new Dictionary<Type, object>()
        {
            { typeof(ISitemapContentProviderFactory), new EmptySitemapContentProviderFactory() },
            { typeof(ISitemapDocumentBuilder), new DefaultSitemapDocumentBuilder() },
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