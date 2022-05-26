using DL.EntitiesV1.Blogs;
using DL.Services.Blogs;
using DL.Services.Comments;
using DL.Services.Polls;
using DL.Services.Reations;
using Microsoft.Extensions.DependencyInjection;

namespace DL
{
    public static class DLServiceRegistration
    {
        public static void RegisterDataAccessServices(this IServiceCollection service)
        {
            service.AddScoped<BlogTimelineService>();
            service.AddScoped<BlogTagService>();
            
            service.AddScoped<PollAnswerService>();
            
            service.AddScoped<ReactionService>();

            service.AddScoped<CommentService>();
        }
    }
}