using System;
using System.Collections.Generic;
using System.Linq;
using Sitemapify.Umbraco.Config;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Sitemapify.Umbraco.Extensions
{
    internal static class ContentExtensions
    {
        public static bool ShowInSitemap(this IPublishedContent node, string propertyAlias = null)
        {
            return !node?.HideFromSitemap(propertyAlias) ?? false;
        }

        public static bool HideFromSitemap(this IPublishedContent node, string propertyAlias = null)
        {
            return node?.GetPropertyValue<bool>(propertyAlias ?? SitemapifyUmbracoContentProviderSettings.Current.ExcludedFromSitemapPropertyAlias, false) ?? false;
        }

        public static bool HideChildrenFromSitemap(this IPublishedContent node, string propertyAlias = null)
        {
            return node?.GetPropertyValue<bool>(propertyAlias ?? SitemapifyUmbracoContentProviderSettings.Current.ExcludedChildrenFromSitemapPropertyAlias, false) ?? false;
        }

        public static IEnumerable<IPublishedContent> DescendantSitemapNodes(this IPublishedContent node, string hideFromSitemapPropertyAlias = null, string hideChildrenFromSitemapPropertyAlias = null)
        {
            if (!node.HideFromSitemap(hideFromSitemapPropertyAlias)) yield return node;
            if (!node.HideChildrenFromSitemap(hideChildrenFromSitemapPropertyAlias))
            {
                foreach (var child in node.Children())
                {
                    foreach (var grandChild in child.DescendantSitemapNodes(hideFromSitemapPropertyAlias, hideChildrenFromSitemapPropertyAlias))
                        yield return grandChild;
                }
            }
        }
    }
}
