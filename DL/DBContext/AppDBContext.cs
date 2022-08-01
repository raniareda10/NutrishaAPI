using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL.Entities;
using DL.EntitiesV1;
using DL.EntitiesV1.Allergies;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Articles;
using DL.EntitiesV1.Blogs.Polls;
using DL.EntitiesV1.Comments;
using DL.EntitiesV1.ContactSupport;
using DL.EntitiesV1.Dairies;
using DL.EntitiesV1.Enum;
using DL.EntitiesV1.Meals;
using DL.EntitiesV1.Measurements;
using DL.EntitiesV1.Media;
using DL.EntitiesV1.Reactions;
using DL.EntitiesV1.Reminders;
using DL.EntitiesV1.Roles;
using DL.EntitiesV1.Users;
using DL.EntityTypeBuilders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DL.DBContext
{
    public sealed class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }


        #region Blogs

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogTag> BlogTag { get; set; }
        public DbSet<Article> Articles { get; set; }

        #endregion

        #region Polls

        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollQuestion> PollQuestions { get; set; }
        public DbSet<PollAnswer> PollAnswers { get; set; }

        #endregion

        #region Reactions

        public DbSet<Reaction> Reactions { get; set; }

        #endregion

        #region Comments

        public DbSet<Comment> Comments { get; set; }

        #endregion

        #region Reminders

        public DbSet<ReminderEntity> Reminders { get; set; }

        #endregion

        #region Allergy

        public DbSet<UserAllergy> UserAllergy { get; set; }

        #endregion

        #region Dislikes

        public DbSet<UserDislikes> UserDislikes { get; set; }

        #endregion

        #region User Preventions

        public DbSet<MobileUserPreventionEntity> UserPreventions { get; set; }

        #endregion

        #region Contact Us

        public DbSet<ContactSupportEntity> ContactSupports { get; set; }
        public DbSet<ContactSupportType> ContactSupportTypes { get; set; }

        #endregion

        #region UserMeasurement

        public DbSet<UserMeasurementEntity> UserMeasurements { get; set; }

        #endregion

        #region Meals

        public DbSet<MealEntity> Meals { get; set; }
        public DbSet<MealPlanEntity> MealPlans { get; set; }
        public DbSet<PlanDayEntity> PlanDays { get; set; }
        public DbSet<PlanDayMenuEntity> PlanDayMenus { get; set; }
        public DbSet<PlanDayMenuMealEntity> PlanDayMenuMeals { get; set; }

        #endregion

        #region Dairies

        public DbSet<DairyEntity> Dairies { get; set; }
        

        #endregion
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBlogs(modelBuilder);
            ConfigureUsers(modelBuilder);
            ConfigureUserMeasurements(modelBuilder);
            ConfigureUserPreventions(modelBuilder);
            ConfigurePermissions(modelBuilder);

            // modelBuilder.Entity<PlanDayEntity>()
            //     .HasOne<MealPlanEntity>()
            //     .WithMany(p => p.PlanDays)
            //     .HasForeignKey(p => p.MealPlanId);
            //
            // modelBuilder.Entity<PlanDayMenuEntity>()
            //     .HasOne<PlanDayEntity>()
            //     .WithMany(p => p.PlanMeals)
            //     .HasForeignKey(p => p.PlanDayId);
            //
            // modelBuilder.Entity<PlanDayMenuMealEntity>()
            //     .HasOne<PlanDayMenuEntity>()
            //     .WithMany(p => p.Meals)
            //     .HasForeignKey(p => p.PlanDayMenuId);
        }
        

        private void ConfigurePermissions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionEntity>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<PermissionEntity>().HasIndex(p => p.Name)
            .IsUnique();
        }

        private void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MUser>()
                .ApplyTotalToJson();

            modelBuilder.Entity<MUser>()
                .Property(m => m.Totals)
                .IsRequired()
                .HasDefaultValue(new Dictionary<string, int>()
                {
                    { TotalKeys.Likes, 0 },
                    { TotalKeys.Comments, 0 },
                });
        }

        private void ConfigureUserPreventions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MobileUserPreventionEntity>()
                .HasIndex(m => m.PreventionType)
                .IsUnique(false);
        }

        private void ConfigureUserMeasurements(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMeasurementEntity>()
                .HasIndex(m => m.MeasurementType)
                .IsUnique(false);
        }

        private void ConfigureBlogs(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property(b => b.Media)
                .HasConversion(
                    media => JsonConvert.SerializeObject(media),
                    media => JsonConvert.DeserializeObject<IList<MediaFile>>(media)
                );

            modelBuilder.Entity<Blog>()
                .ApplyTotalToJson();

            modelBuilder.Entity<Comment>()
                .ApplyTotalToJson();
        }


        #region Legacy Entities

        public DbSet<MUser> MUser { get; set; }
        public DbSet<MRole> MRoles { get; set; }
        public DbSet<MUserRoles> MUserRoles { get; set; }
        public DbSet<MNotificationType> MNotificationType { get; set; }
        public DbSet<MGoalType> MGoalType { get; set; }
        public DbSet<MAllergy> MAllergy { get; set; }
        public DbSet<MFrequency> MFrequency { get; set; }
        public DbSet<MGender> MGender { get; set; }
        public DbSet<MRisk> MRisk { get; set; }
        public DbSet<MJourneyPlan> MJourneyPlan { get; set; }
        public DbSet<MNotification> MNotification { get; set; }
        public DbSet<MNotificationUser> MNotificationUser { get; set; }
        public DbSet<MContactUs> MContactUs { get; set; }
        public DbSet<MGroup> MGroups { get; set; }
        public DbSet<MUserGroup> MUserGroups { get; set; }
        public DbSet<MRolesGroup> MRolesGroups { get; set; }
        public DbSet<MVerfiyCode> MVerfiyCode { get; set; }

        public DbSet<MGoal> MGoal { get; set; }
        public DbSet<MUserAllergy> MUserAllergy { get; set; }
        public DbSet<MUserGoal> MUserGoal { get; set; }
        public DbSet<MUserRisk> MUserRisk { get; set; }
        public DbSet<MMealType> MMealType { get; set; }
        public DbSet<MFoodSteps> MFoodSteps { get; set; }
        public DbSet<MIngredient> MIngredient { get; set; }

        public DbSet<MMeal> MMeal { get; set; }
        public DbSet<MDislikeMeal> MDislikeMeal { get; set; }
        public DbSet<MMealIngredient> MMealIngredient { get; set; }
        public DbSet<MUserMeal> MUserMeal { get; set; }
        public DbSet<MMealSteps> MMealSteps { get; set; }

        public DbSet<MVideo> MVideo { get; set; }
        public DbSet<MBlogType> MBlogType { get; set; }
        public DbSet<MMediaType> MMediaType { get; set; }
        public DbSet<MAttachmentType> MAttachmentType { get; set; }

        public DbSet<SecUser> SecUser { get; set; }
        public DbSet<MSplash> MSplash { get; set; }

        #endregion
    }
}