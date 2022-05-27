using DL.EntitiesV1.Blogs;
using DL.Services.Blogs;
using DL.Services.Blogs.Articles;
using DL.Services.Blogs.BlogDetails;
using DL.Services.Comments;
using DL.Services.Polls;
using DL.Services.Reactions;
using Microsoft.Extensions.DependencyInjection;

namespace DL
{
    public static class DLServiceRegistration
    {
        public static void RegisterDataAccessServices(this IServiceCollection service)
        {
            service.AddScoped<BlogService>();
            service.AddScoped<BlogTagService>();
            service.AddTransient<BlogDetailsFactory>();
            service.AddTransient<ArticleService>();

            service.AddScoped<PollAnswerService>();
            
            service.AddScoped<ReactionService>();

            service.AddScoped<CommentService>();
        }
    }
}