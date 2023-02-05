using System.Linq;
using System.Threading.Tasks;
using DL;
using DL.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NutrishaAPI.Extensions;

namespace NutrishaAPI.Attributes
{
    public class HasPermissionOnlyAttribute : ActionFilterAttribute
    {
        private readonly string _permission;

        public HasPermissionOnlyAttribute(string permission)
        {
            _permission = permission;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDBContext>();
            var userContext = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

            if (await dbContext.MUserRoles.AnyAsync(r =>
                    r.AdminUserId == userContext.UserId &&
                    r.Role.RolePermissions.Any(p => p.Permission.Name == _permission)))
            {
                await next();
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}