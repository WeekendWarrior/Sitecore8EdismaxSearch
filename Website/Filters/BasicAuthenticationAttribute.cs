using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using log4net;
using Sitecore8EdismaxSearch.Website.Configuration;

namespace Sitecore8EdismaxSearch.Website.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Authorizes the incoming request.
        /// </summary>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AuthenticationHeaderValue authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string userName = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                // Compare request header username & pw vs the app settings values.
                var isValid = userName == AppSettings.Authentication_Username 
                    && password == AppSettings.Authentication_Password;

                if (isValid)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(userName), null);
                    Thread.CurrentPrincipal = principal;

                    return;
                }
            }

            HandleUnauthorized(actionContext);
        }

        /// <summary>
        /// Handles 401 Unauthorized requests.
        /// </summary>
        private static void HandleUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = '" + actionContext.Request.RequestUri.AbsoluteUri + ":");

            // Log the attempt.
            string authenticationToken = actionContext.Request.Headers?.Authorization?.Parameter;

            string decodedAuthenticationToken = !string.IsNullOrWhiteSpace(authenticationToken)
                ? Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken))
                : string.Empty;

            _log.InfoFormat("Failed authentication. Url = '{0}'; authenticationToken = '{1}'; decodedAuthenticationToken = '{2}'",
                actionContext.Request.RequestUri.AbsoluteUri,
                authenticationToken,
                decodedAuthenticationToken
            );
        }
    }
}