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
            InitializeFields(httpContextAccessor);
        }

        public int UserId { get; set; }
        public string Locale { get; set; }

        
        private void InitializeFields(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext == null) return;
            
            Locale = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString() ?? "en";
            
            var userIdClaims = httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaims != null)
            {
                UserId = int.Parse(userIdClaims.Value);
            }
        }
    }
}