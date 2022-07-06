using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Responses;

namespace NutrishaAPI.Extensions
{
    public static class ResponseExtensions
    {
        public static IActionResult InvalidResult(string error)
        {
            return new BadRequestObjectResult(new BaseResponse<object>()
            {
                Done = false,
                ErrorMessage = error,
                StatusCode = 400
            });
        }
        
        public static IActionResult InvalidResult(IEnumerable<string> errors)
        {
            return new BadRequestObjectResult(new BaseResponse<object>()
            {
                Done = false,
                ErrorMessage = errors.First(),
                StatusCode = 400
            });
        }
    }
}