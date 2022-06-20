using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NutrishaAPI.Responses;

namespace NutrishaAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
               await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"something goes wrong at {httpContext.Request.Path}?{httpContext.Request.QueryString}");
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponse<object>()
                {
                    Done = false,
                    ErrorMessage = JsonConvert.SerializeObject(ex.Message),
                    StatusCode = StatusCodes.Status500InternalServerError
                }));
            }
        }

    }
}