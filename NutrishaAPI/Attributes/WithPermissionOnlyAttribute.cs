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
    public class WithPermissionOnlyAttribute : ActionFilterAttribute
    {
        private readonly string[] _permissions;

        public WithPermissionOnlyAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isAdmin = context.HttpContext
                .User
                .IsAdmin();

            if (!isAdmin)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDBContext>();
            var userContext = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();


            var userRoleIds = await dbContext
                .MUserRoles
                .Where(r => r.UserId == userContext.UserId)
                .Select(r => r.Role.Id)
                .ToListAsync();

            var userPermissions = await dbContext.RolePermissions.Where(m => userRoleIds.Contains(m.RoleId))
                .Select(m => m.Permission.Name).ToListAsync();
            
            
        }
    }
}