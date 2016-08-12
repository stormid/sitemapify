using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Sitemapify.Extensions;

namespace Sitemapify
{
    public class DefaultSitemapDocumentBuilder : ISitemapDocumentBuilder
    {
        public void BuildSitemapXmlStream(IEnumerable<SitemapUrl> sitemapUrls, TextWriter writer)
        {
            if(writer == null) throw new ArgumentNullException(nameof(writer));

            var elements = sitemapUrls.Select(url => url.ToXElement()).ToList();
            var assemblyName = GetType().Assembly.GetName();
            var description =
                $"Sitemapify {GetType().Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description}";
            var headerComments = new XComment($" {description} v{assemblyName.Version} at {DateTime.UtcNow.ToString("O")} ");

            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), headerComments, new XElement(XName.Get("urlset", SitemapUrl.SitemapNs), elements));

            document.Save(writer, SaveOptions.OmitDuplicateNamespaces);
        }
    }
}