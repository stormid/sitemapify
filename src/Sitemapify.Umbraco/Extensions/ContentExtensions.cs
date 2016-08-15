using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Sitemapify.Umbraco.Extensions
{
    internal static class ContentExtensions
    {
        public static bool HideFromSitemap(this IPublishedContent node, string propertyAlias)
        {
            return node?.GetPropertyValue<bool>(propertyAlias, false) ?? false;
        }

        public static bool HideChildrenFromSitemap(this IPublishedContent node, string propertyAlias)
        {
            return node?.GetPropertyValue<bool>(propertyAlias, false) ?? false;
        }

        public static IEnumerable<IPublishedContent> AllChildren(this IPublishedContent node, Func<IPublishedContent, bool> predicate = null, Func<IPublishedContent, bool> includeChildren = null)
        {
            if (predicate?.Invoke(node) ?? true) yield return node;
            if (includeChildren?.Invoke(node) ?? true)
            {
                foreach (var child in node.Children(predicate))
                {
                    foreach (var grandChild in child.AllChildren(predicate))
                        yield return grandChild;
                }
            }
        }
    }
}
