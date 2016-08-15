Sitemapify.Umbraco
~~~~~~~~~~

Provides a ASP.NET HttpHandler to assist with generating a dynamic sitemap.xml file for Umbraco CMS content nodes


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