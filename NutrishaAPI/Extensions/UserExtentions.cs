using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace NutrishaAPI.Extensions
{
    public static class UserExtentions
    {
        public static int? GetUserId(this ClaimsPrincipal claims)
        {
            var userIdClaims = claims.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaims != null)
            {
                return int.Parse(userIdClaims.Value);
            }

            return null;
        }
    }
}