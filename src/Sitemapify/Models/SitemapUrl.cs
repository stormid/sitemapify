using System;

namespace Sitemapify.Models
{
    public struct SitemapUrl
    {
        public const string SitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";

        /// <summary>
        /// REQUIRED: The location URI of a document. The URI must conform to RFC 2396 (http://www.ietf.org/rfc/rfc2396.txt).
        /// </summary>
        public string Loc { get; }

        /// <summary>
        /// OPTIONAL: The date the document was last modified. 
        /// The date must conform to the W3C DATETIME format (http://www.w3.org/TR/NOTE-datetime). 
        /// Example: 2005-05-10 Lastmod may also contain a timestamp. 
        /// Example: 2005-05-10T17:33:30+08:00
        /// </summary>
        public DateTime? Lastmod { get; }

        /// <summary>
        /// OPTIONAL: Indicates how frequently the content at a particular URL is likely to change. 
        /// The value "always" should be used to describe documents that change each time they are accessed. 
        /// The value "never" should be used to describe archived URLs. Please note that web crawlers may not necessarily crawl pages marked "always" more often. Consider this element as a friendly suggestion and not a command.
        /// </summary>
        public SitemapChangeFrequency? ChangeFreq { get; }

        /// <summary>
        /// OPTIONAL: The priority of a particular URL relative to other pages on the same site. 
        /// The value for this element is a number between 0.0 and 1.0 where 0.0 identifies the lowest priority page(s). 
        /// The default priority of a page is 0.5. Priority is used to select between pages on your site. Setting a priority of 1.0 for all URLs will not help you, as the relative priority of pages on your site is what will be considered.
        /// </summary>
        public double? Priority { get; }

        internal SitemapUrl(string loc, DateTime? lastmod = null, SitemapChangeFrequency? changeFreq = null, double? priority = null)
        {
            Loc = loc;
            Lastmod = lastmod;
            ChangeFreq = changeFreq;
            Priority = priority;
        }

        public static SitemapUrl Create(string loc, DateTime? lastmod = null, SitemapChangeFrequency? changeFreq = null, double? priority = null)
        {
            return new SitemapUrl(loc, lastmod, changeFreq, priority);
        }
    }
}