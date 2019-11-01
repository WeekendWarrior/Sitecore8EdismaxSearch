using System.Web.Http;
using Sitecore8EdismaxSearch.Website.Services.SearchRepository.Config;

namespace Sitecore8EdismaxSearch.Website
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Register IoT / Unity.
            UnityConfig.RegisterTypes(UnityConfig.Container);

            // Register Web API 2.
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Register Solr Containers.
            SolrSearchConfig.Configure(SolrSearchConfig.Container);
        }
    }
}
