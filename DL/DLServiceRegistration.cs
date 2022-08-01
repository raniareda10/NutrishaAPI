using DL.EntitiesV1.Blogs;
using DL.Repositories;
using DL.Repositories.Allergy;
using DL.Repositories.Blogs;
using DL.Repositories.Blogs.Articles;
using DL.Repositories.Blogs.BlogDetails;
using DL.Repositories.Blogs.Polls;
using DL.Repositories.BlogVideo;
using DL.Repositories.Comments;
using DL.Repositories.ContactSupport;
using DL.Repositories.Dislikes;
using DL.Repositories.MealPlan;
using DL.Repositories.Meals;
using DL.Repositories.MobileUser;
using DL.Repositories.Polls;
using DL.Repositories.Profiles;
using DL.Repositories.Reactions;
using DL.Repositories.Reminders;
using DL.Repositories.UserMeasurement;
using DL.Repositories.Users;
using DL.Repositories.Users.Admins;
using DL.Services.Sms;
using DL.Services.Sms.Models;
using DL.StorageServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DL.Repositories.Roles;

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
            service.AddScoped<DislikesMealService>();
            service.AddScoped<MobileProfileService>();
            service.AddScoped<ReminderService>();
            service.AddScoped<MealsRepository>();


            service.AddScoped<UserMeasurementRepository>();
            service.AddScoped<MobileUserRepository>();

            service.AddScoped<ContactSupportService>();
            service.AddScoped<BlogVideoRepository>();
            service.AddScoped<AdminUserRepository>();
            service.AddScoped<RollRepository>();
            service.AddScoped<DairyRepository>();
            service.AddScoped<MobileMealPlanRepository>();
            service.AddScoped<MealPlanRepository>();

            service.AddSingleton<IStorageService, StorageService>();

            #region Users

            service.AddScoped<AdminAuthService>();
            service.AddScoped<TokenService>();

            #endregion

            service.RegisterServices();
        }


        private static void RegisterServices(this IServiceCollection service)
        {
            service.AddSingleton<ISmsGetaway, SmsGetaway>();
            service.AddSingleton(options =>
            {
                var config = options.GetRequiredService<IConfiguration>();
                return config.GetSection("SmsConfiguration").Get<SmsConfiguration>();
            });
        }
    }
}