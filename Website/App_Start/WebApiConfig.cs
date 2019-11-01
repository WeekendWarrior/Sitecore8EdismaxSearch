using Sitecore8EdismaxSearch.Website.Filters;
using Sitecore8EdismaxSearch.Website.Loggers;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Sitecore8EdismaxSearch.Website
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes.
            config.MapHttpAttributeRoutes();

            // Add default route.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Register Basic Auth filter.
            config.Filters.Add(new BasicAuthenticationAttribute());

            // Register Error Handler.
            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());
        }
    }
}
