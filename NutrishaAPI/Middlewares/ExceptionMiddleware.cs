using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NutrishaAPI.Responses;

namespace NutrishaAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
               await _next(httpContext);
            }
            catch (Exception ex)
            {
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