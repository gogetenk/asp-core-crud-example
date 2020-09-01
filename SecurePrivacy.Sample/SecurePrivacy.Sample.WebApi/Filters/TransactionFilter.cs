using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace WebApi.Filters
{
    /// <summary>
    /// Filter that will trigger the opening and closure of an abstract transaction for each requests. Suitable for any kind of transaction enabled ORM.
    /// </summary>
    public class TransactionFilter : IAsyncActionFilter
    {
        private readonly IClientSessionHandle _session;
        private readonly ILogger<TransactionFilter> _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="session">The scoped session (one session per request)</param>
        /// <param name="logger">The logger</param>
        public TransactionFilter(IClientSessionHandle session, ILogger<TransactionFilter> logger)
        {
            _session = session;
            _logger = logger;
        }

        /// <summary>
        /// Executed on each HTTP request
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Should not happen, but let's stay safe
            if (next is null)
                return;

            _logger.LogDebug($"[Transaction] Starting transaction [ID: {_session.ServerSession.Id}]");
            _session.StartTransaction();

            var executedContext = await next.Invoke();
            if (executedContext.Exception is null)
            {
                _logger.LogDebug($"[Transaction] Commiting transaction [ID: {_session.ServerSession.Id}]");
                await _session.CommitTransactionAsync();
            }
            else
            {
                // If an exception happens anywhere, we just abort the transation
                _logger.LogDebug($"[Transaction] Aborting transaction [ID: {_session.ServerSession.Id}]");
                await _session.AbortTransactionAsync();
            }

        }
    }
}
