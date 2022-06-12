using DL.EntitiesV1.Blogs;
using DL.Services.Allergy;
using DL.Services.Blogs;
using DL.Services.Blogs.Articles;
using DL.Services.Blogs.BlogDetails;
using DL.Services.Blogs.Polls;
using DL.Services.Comments;
using DL.Services.ContactSupport;
using DL.Services.Polls;
using DL.Services.Profiles;
using DL.Services.Reactions;
using DL.Services.Reminders;
using DL.Services.Users;
using DL.Services.Users.Admins;
using DL.StorageServices;
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
            service.AddTransient<PollService>();

            service.AddScoped<PollAnswerService>();

            service.AddScoped<ReactionService>();

            service.AddScoped<CommentService>();

            service.AddScoped<AllergyService>();
            service.AddScoped<MobileProfileService>();
            service.AddScoped<ReminderService>();
            
            
            service.AddScoped<ContactSupportService>();

            service.AddScoped<IStorageService, StorageService>();

            #region Users

            service.AddScoped<AdminUserService>();
            service.AddScoped<TokenService>();

            #endregion
        }
    }
}