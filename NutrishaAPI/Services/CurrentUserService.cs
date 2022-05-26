using System.Linq;
using System.Security.Claims;
using DL;
using Microsoft.AspNetCore.Http;

namespace NutrishaAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = int.Parse(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        public int UserId { get; set; }
    }
}