using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NutrishaAPI.Extensions;

namespace NutrishaAPI.Attributes
{
    public class OnlyMobileUsersAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isAdmin = context.HttpContext.User.IsAdmin();

            if (!isAdmin)
            {
                return base.OnActionExecutionAsync(context, next);
            }
            
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }
    }
}