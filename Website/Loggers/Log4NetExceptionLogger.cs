using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using log4net;

namespace Sitecore8EdismaxSearch.Website.Loggers
{
    /// <summary>
    /// Custom implementation of <see cref="ExceptionLogger"/> that writes exceptions to log4net.
    /// </summary>
    public class Log4NetExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// Logs an exception.
        /// </summary>
        public override void Log(ExceptionLoggerContext context)
        {
            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            log.Error("Uncaught Exception", context.ExceptionContext.Exception);

            base.Log(context);
        }

        /// <summary>
        /// Logs an async exception.
        /// </summary>
        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            log.Error("Uncaught Async Exception", context.ExceptionContext.Exception);

            return base.LogAsync(context, cancellationToken);
        }
    }
}