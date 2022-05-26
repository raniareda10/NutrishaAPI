using DL;
using Microsoft.Extensions.DependencyInjection;
using NutrishaAPI.Services;

namespace NutrishaAPI.ServicesRegistrations
{
    public static class CurrentUserServicesRegistration
    {
        public static void RegisterCurrentUserServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }
    }
}