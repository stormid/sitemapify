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