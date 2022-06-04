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
            Locale = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString() ?? "en";
        }

        public int UserId { get; set; }
        public string Locale { get; set; }
    }
}