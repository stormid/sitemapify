# Sitemapify
Provides a ASP.NET HttpHandler to assist with generating a dynamic sitemap.xml file

## Installation
The installation process should have updated your web.config file to include a httphandler to process requests to `sitemap.xml`, it should look similar to:

```xml
<configuration>
  <system.webServer>
    <handlers>
      <add name="Sitemapify" path="sitemap.xml" verb="GET" type="Sitemapify.SitemapifyHttpHandler, Sitemapify" />
    </handlers>
  </system.webServer>
</configuration>
```

## Customisation
The sitemap handler can be customised by altering the base configuration via the ```Sitemapify.Configure``` static configuration class.  

```c#
Sitemapify.Configure(c => c
  .UsingContentProvider(new YourContentProvider())
  .UsingCacheProvider(new YourCacheProvider())
  .UsingDocumentBuilder(new YourDocumentBuilder())
);
```

Sitemapify is split into providers that are responsible for elements of the sitemap generation.

### Content Provider (`ISitemapifyContentProvider`)
An implementation of a content provider supplies Sitemapify with a collection of `SitemapUrl` objects representing the nodes to output within the sitemap.

```c#
public interface ISitemapContentProvider
{
    IEnumerable<SitemapUrl> GetSitemapUrls();
    bool Cacheable { get; }
    DateTime CacheUntil { get; }
}
```

### Cache Provider (`ISitemapifyCacheProvider`)
An implementation of a cache provider allows customisation of the caching mechanism used to optimise delivery of the sitemap to the browser.  Once the sitemap document is generated it will be cached via the configured implementation of this interface.

```c#
public interface ISitemapCacheProvider
{
    void Add(XDocument sitemapContent, DateTimeOffset? expires);
    bool IsCached { get; }
    XDocument Get();
    void Remove();
}
```

### Document Builder (`ISitemapDocumentBuilder`)
An implementation of the `ISitemapDocumentBuilder` provides allows full customisation of the sitemap document itself.  The default implementation generates a fully compliant sitemap document based on the official [sitemap.xsd](http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd).

```c#
public interface ISitemapDocumentBuilder
{
    XDocument BuildSitemapXmlDocument(IEnumerable<SitemapUrl> sitemapUrls);
}
```
