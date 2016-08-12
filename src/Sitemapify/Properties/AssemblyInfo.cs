using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using Sitemapify.Config;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Sitemapify")]
[assembly: AssemblyDescription("https://github.com/stormid/sitemapify")]
[assembly: AssemblyCompany("Storm ID")]
[assembly: AssemblyProduct("Sitemapify")]
[assembly: AssemblyCopyright("Copyright © Storm ID 2016")]

[assembly: ComVisible(false)]


[assembly: PreApplicationStartMethod(typeof(SitemapifyConfigure), "Initialise")]
