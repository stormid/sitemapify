using System;
using System.Collections.Generic;
using System.Linq;
using Sitemapify.Models;

namespace Sitemapify.Providers.Impl
{
    public abstract class AbstractSitemapContentProvider<T> : ISitemapContentProvider
    {
        public IEnumerable<SitemapUrl> GetSitemapUrls(Uri baseUrl)
        {
            var overrideBaseUrl = GetBaseUrl(baseUrl);
            var entries = GetSitemapEntries();
            if (entries == null)
            {
                return Enumerable.Empty<SitemapUrl>();
            }

            return entries
                .Where(WhereCore)
                .Select(c => CreateSitemapUrl(c, overrideBaseUrl))
                .ToList();
        }

        /// <summary>
        /// Returns a <see cref="SitemapUrl"/> composed from <see cref="entry"/>.
        /// </summary>
        /// <param name="entry">An object that is expected to be represented in the sitemap</param>
        /// <param name="baseUri">The host of the request made to the sitemap.xml</param>
        /// <returns></returns>
        protected abstract SitemapUrl CreateSitemapUrl(T entry, Uri baseUri);

        private bool WhereCore(T candidate)
        {
            if (candidate == null)
            {
                return false;
            }
            return Where(candidate);
        }

        /// <summary>
        /// Allows custom logic to be used to determine whether a node should be included in the sitemap, this occurs after all built-in determinations
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        protected virtual bool Where(T candidate)
        {
            return true;
        }

        /// <summary>
        /// Must return an absolute uri, otherwise the sitemap will be returned blank
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        protected virtual Uri GetBaseUrl(Uri baseUri)
        {
            return baseUri;
        }

        /// <summary>
        /// Retrieves a list of <see cref="T"/> containing entities that should be included in the sitemap
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<T> GetSitemapEntries();

        public virtual bool Cacheable { get; } = true;
        public virtual DateTime CacheUntil { get; } = DateTime.UtcNow.AddHours(1);
    }
}