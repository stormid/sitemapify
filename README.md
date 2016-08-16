# Sitemapify
Provides a ASP.NET HttpHandler to assist with generating a dynamic sitemap.xml file

There is also an Umbraco specific extension to Sitemapify that supports building the sitemap from Umbraco CMS content.  See [Sitemapify.Umbraco](#sitemapifyumbraco-configuration) for further documentation

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

## Sitemapify.Umbraco Configuration

Once you have installed Sitemapify.Umbraco you can create an ApplicationEventHandler to configure Sitemapify to use the available "Umbraco Content Provider" to generate sitemap url that form the sitemap.xml.

```c#
public class SitemapifyApplicationEventHandler : ApplicationEventHandler
{
    protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        Configure.With(config => config
          .UsingContentProvider(new SitemapifyUmbracoContentProvider())
        );
    }

    protected override bool ExecuteWhenApplicationNotConfigured { get; } = false;
    protected override bool ExecuteWhenDatabaseNotConfigured { get; } = false;
}
```

By default the Umbraco content provider will look for content node properties with specific names to determine whether a particular node should be included in the generated sitemap file, these property names are:

* `umbracoNaviHide`	- True/False to determine whether this node is included (Default: false)
* `sitemapifyExcludeChildren`	- True/False to determine whether to ignore all child nodes below this node (Default: false)

If you want to alter these property aliases you can override them via application settings:

```xml
<appSettings>
	<add key="Sitemapify.Umbraco:ExcludedFromSitemapPropertyAlias" value="newPropertyAlias" />
	<add key="Sitemapify.Umbraco:ExcludedChildrenFromSitemapPropertyAlias" value="anotherPropertyAlias" />
</appSettings>
```
