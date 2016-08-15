Sitemapify
~~~~~~~~~~

Provides a ASP.NET HttpHandler to assist with generating a dynamic sitemap.xml file


Project website
~~~~~~~~~~~~~~~

Visit https://github.com/stormid/Sitemapify for documentation and issues


Installation
~~~~~~~~~~~~

The installation process should have updated your web.config file to include a httphandler to process requests to /sitemap.xml, it should look similar to:

<configuration>
  <system.webServer>
    <handlers>
      <add name="Sitemapify" path="sitemap.xml" verb="GET" type="Sitemapify.SitemapifyHttpHandler, Sitemapify" />
    </handlers>
  </system.webServer>
</configuration>


Configuration
~~~~~~~~~~~~~

Once installed you can create an ApplicationEventHandler to configure Sitemapify to use the available "Umbraco Content Provider" to generate sitemap url that form the sitemap.xml:

public class SitemapifyApplicationEventHandler : ApplicationEventHandler
{
    protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        Configure.With(config => config.UsingContentProvider(new SitemapifyUmbracoContentProvider()));
    }

    protected override bool ExecuteWhenApplicationNotConfigured { get; } = false;
    protected override bool ExecuteWhenDatabaseNotConfigured { get; } = false;
}

By default the Umbraco content provider will look for content node properties with specific names to determine whether a particular node should be included in the generated sitemap file, these property names are:

* umbracoNaviHide				- True/False to determine whether this node is included (Default: false)
* sitemapifyExcludeChildren		- True/False to determine whether to ignore all child nodes below this node (Default: false)

If you want to alter these property aliases you can override them via application settings:

<appSettings>
	<add key="Sitemapify.Umbraco:ExcludedFromSitemapPropertyAlias" value="newPropertyAlias" />
	<add key="Sitemapify.Umbraco:ExcludedChildrenFromSitemapPropertyAlias" value="anotherPropertyAlias" />
</appSettings>