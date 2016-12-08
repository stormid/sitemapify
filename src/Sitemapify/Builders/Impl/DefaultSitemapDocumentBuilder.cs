using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Sitemapify.Extensions;
using Sitemapify.Models;

namespace Sitemapify.Builders.Impl
{
    [DebuggerStepThrough]
    public class DefaultSitemapDocumentBuilder : ISitemapDocumentBuilder
    {
        public XDocument BuildSitemapXmlDocument(IEnumerable<SitemapUrl> sitemapUrls)
        {
            var elements = sitemapUrls.Select(url => url.ToXElement()).ToList();
            var assemblyName = GetType().Assembly.GetName();
            var description =
                $"Sitemapify - {GetType().Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description}";
            var headerComments = new []
            {
                new XComment($" {description} "),
                new XComment($" Version: {assemblyName.Version} "),
                new XComment($" Generated: {DateTime.UtcNow:O} ")

            };

            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), headerComments, new XElement(XName.Get("urlset", SitemapUrl.SitemapNs), elements));
        }
    }
}