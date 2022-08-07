using System.Linq;
using System.Security.Claims;
using DL.Secuirty.Enums;

namespace NutrishaAPI.Extensions
{
    public static class UserClaimsExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal userPrincipal)
        {
            var adminClaim = userPrincipal
                .Claims
                .FirstOrDefault(c => c.Type == ApplicationClaimTypes.IsAdmin);

            return adminClaim != null && bool.TryParse(adminClaim.Value, out var isAdmin) && isAdmin;
        }
    }
}