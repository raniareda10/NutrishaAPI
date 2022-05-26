using DL.EntitiesV1.Blogs;
using Microsoft.Extensions.DependencyInjection;

namespace DL
{
    public static class DLServiceRegistration
    {
        public static void RegisterDataAccessServices(this IServiceCollection service)
        {
            service.AddScoped<BlogTimelineService>();
        }
    }
}