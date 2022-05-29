using System.Linq;
using System.Threading.Tasks;
using BL.Security.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NutrishaAPI.Attributes
{
    public class OnlyAdminsAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var adminClaim = context.HttpContext
                .User
                .Claims
                .First(c => c.Type == ApplicationClaimTypes.IsAdmin);

            var isAdmin = bool.Parse(adminClaim.Value);

            if (isAdmin)
            {
                return base.OnActionExecutionAsync(context, next);
            }

            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }
    }
}