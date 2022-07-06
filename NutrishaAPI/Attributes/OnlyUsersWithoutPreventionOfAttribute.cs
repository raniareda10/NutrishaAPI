using System;
using System.Threading.Tasks;
using DL.DBContext;
using DL.EntitiesV1.Users;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NutrishaAPI.Extensions;

namespace NutrishaAPI.Attributes
{
    public class OnlyUsersWithoutPreventionOfAttribute : ActionFilterAttribute
    {
        private readonly MobilePreventionType _preventionType;

        public OnlyUsersWithoutPreventionOfAttribute(MobilePreventionType preventionType)
        {
            _preventionType = preventionType;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.GetUserId();

            if (!userId.HasValue)
            {
                throw new Exception();
            }

            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

                var isUserPrevented =
                    await dbContext.UserPreventions.AnyAsync(p =>
                        p.PreventionType == _preventionType && p.UserId == userId);

                if (isUserPrevented)
                {
                    context.Result = ResponseExtensions.InvalidResult(NonLocalizedErrorMessages.ActionPrevented);
                    return;
                }

                await base.OnActionExecutionAsync(context, next);
            }
        }
    }
}