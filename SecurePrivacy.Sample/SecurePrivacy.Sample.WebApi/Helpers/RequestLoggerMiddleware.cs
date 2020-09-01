using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace WebApi.Helpers
{
    /// <summary>
    /// Middleware class for logging incoming HTTP requests.
    /// </summary>
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private const string _RequestBodyString = "Body";
        private const string _RequestFullPathString = "FullPath";
        private const string _RequestMethodString = "Method";
        private const string _RequestHeadersString = "Headers";

        /// <summary>
        /// Constructor for <see cref="RequestLoggerMiddleware"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Core middleware code.
        /// Logs incoming HTTP requests.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var request = await ReadRequestAsync(context.Request);
            _logger.LogTrace("Incoming request {HttpRequest}", request);

            using (_logger.BeginScope(new Dictionary<string, string> { { _RequestMethodString, request[_RequestMethodString].ToString() }, { _RequestFullPathString, request[_RequestFullPathString].ToString() } }))
            {
                await _next(context);
            }
        }

        /// <summary>
        /// Reads an incoming http request and extracts its path, method, headers and body.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, object>> ReadRequestAsync(HttpRequest request)
        {
            using (var requestBodyStream = new MemoryStream())
            {
                await request.Body.CopyToAsync(requestBodyStream);
                request.Body.Seek(0, SeekOrigin.Begin);
                requestBodyStream.Seek(0, SeekOrigin.Begin);

                var requestBody = await new StreamReader(requestBodyStream).ReadToEndAsync();

                return new Dictionary<string, object>()
                {
                    { _RequestFullPathString, request.GetDisplayUrl() },
                    { _RequestMethodString, request.Method },
                    { _RequestHeadersString, request.Headers.ToDictionary(x => x.Key, y => y.Value) },
                    { _RequestBodyString, requestBody }
                };
            }
        }
    }
}
