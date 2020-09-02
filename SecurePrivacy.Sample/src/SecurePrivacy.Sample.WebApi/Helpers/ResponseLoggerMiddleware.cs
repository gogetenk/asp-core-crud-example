using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApi.Helpers
{
    /// <summary>
    /// Middleware class for logging HTTP responses.
    /// </summary>
    public class ResponseLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private const string _ResponseStatusCode = "StatusCode";
        private const string _ResponseBody = "Body";
        private const string _ResponseHeadersString = "Headers";

        /// <summary>
        /// Constructor for <see cref="ResponseLoggerMiddleware"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ResponseLoggerMiddleware(RequestDelegate next, ILogger<ResponseLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Core middleware code.
        /// Logs outgoing HTTP responses.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);
                var response = await ReadResponseAsync(context.Response);
                _logger.LogTrace("Outgoing response {HttpResponse}", response);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        /// <summary>
        /// Reads an outgoing http response and extracts its http status code, headers, and body.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, object>> ReadResponseAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return new Dictionary<string, object>()
                {
                    { _ResponseStatusCode, response.StatusCode.ToString() },
                    { _ResponseHeadersString, response.Headers.ToDictionary(x => x.Key, y => y.Value) },
                    { _ResponseBody, text }
                };
        }
    }
}
