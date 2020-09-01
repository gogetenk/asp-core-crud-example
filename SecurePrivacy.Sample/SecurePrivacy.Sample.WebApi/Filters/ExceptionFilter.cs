using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SecurePrivacy.Sample.Bll.Impl.Exceptions;
using SecurePrivacy.Sample.Dal.Exceptions;
using SecurePrivacy.Sample.Dto;

namespace WebApi.Filters
{
    /// <summary>
    /// This handler is an exception filter that handles explicit json response for the client.
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        private const string _DalError = "An error occured in the database layer.";
        private const string _BusinessError = "A business error occured.";
        private const string _FatalError = "A fatal error occured.";

        /// <summary>
        /// Build a new instance of <see cref="ExceptionFilter"/>.
        /// </summary>
        /// <param name="logger"></param>
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get access to exception context to log and enrich the response.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case DalException dalException:
                    _logger.LogError(dalException, _DalError);
                    context.Result = CreateErrorResponse($"{_DalError}{Environment.NewLine}{dalException.Message}");
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                case BusinessException businessException:
                    _logger.LogError(businessException, _BusinessError);
                    context.Result = CreateErrorResponse($"{_BusinessError}{Environment.NewLine}{businessException.Message}");
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    _logger.LogCritical(context.Exception, _FatalError);
                    context.Result = CreateErrorResponse($"{_FatalError}{Environment.NewLine}{context.Exception.Message}");
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            base.OnException(context);
        }

        private JsonResult CreateErrorResponse(string errorMessage)
        {
            var responseDto = new ErrorResponseDto
            {
                ErrorMessage = errorMessage
            };
            return new JsonResult(responseDto);
        }
    }
}
