using System.Threading.Tasks;
using DL;
using DL.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NutrishaAPI.Responses;

namespace NutrishaAPI.Attributes
{
    public class AlloyNotBannedOnlyAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDBContext>();
            var userContext = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

            var isBanned = await dbContext.MUser.AnyAsync(m => m.IsBanned && m.Id == userContext.UserId);
            if (!isBanned)
            {
                await next();
                return;
            }

            context.HttpContext.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
            await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponse<object>()
            {
                Done = false,
                ErrorMessage = "You can't perform this action because you are banned",
                StatusCode = 400
            }));
        }
    }
}