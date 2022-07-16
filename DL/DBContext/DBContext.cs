using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL.EntitiesV1;
using DL.EntitiesV1.Allergies;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Articles;
using DL.EntitiesV1.Blogs.Polls;
using DL.EntitiesV1.Comments;
using DL.EntitiesV1.ContactSupport;
using DL.EntitiesV1.Enum;
using DL.EntitiesV1.Meals;
using DL.EntitiesV1.Measurements;
using DL.EntitiesV1.Media;
using DL.EntitiesV1.Reactions;
using DL.EntitiesV1.Reminders;
using DL.EntitiesV1.Users;
using DL.EntityTypeBuilders;
using Newtonsoft.Json;
using DL.EntitiesV1.Roles;

namespace DL.DBContext
{
    public class AppDBContext : DbContext
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
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<PlanMeal> PlanMeals { get; set; }

        #endregion

        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBlogs(modelBuilder);
            ConfigureUsers(modelBuilder);
            ConfigureUserMeasurements(modelBuilder);
            ConfigureUserPreventions(modelBuilder);
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

        public virtual DbSet<MUser> MUser { get; set; }
        public virtual DbSet<MRole> MRoles { get; set; }
        public virtual DbSet<MUserRoles> MUserRoles { get; set; }
        public virtual DbSet<MNotificationType> MNotificationType { get; set; }
        public virtual DbSet<MGoalType> MGoalType { get; set; }
        public virtual DbSet<MAllergy> MAllergy { get; set; }
        public virtual DbSet<MFrequency> MFrequency { get; set; }
        public virtual DbSet<MGender> MGender { get; set; }
        public virtual DbSet<MRisk> MRisk { get; set; }
        public virtual DbSet<MJourneyPlan> MJourneyPlan { get; set; }
        public virtual DbSet<MNotification> MNotification { get; set; }
        public virtual DbSet<MNotificationUser> MNotificationUser { get; set; }
        public virtual DbSet<MContactUs> MContactUs { get; set; }
        public virtual DbSet<MGroup> MGroups { get; set; }
        public virtual DbSet<MUserGroup> MUserGroups { get; set; }
        public virtual DbSet<MRolesGroup> MRolesGroups { get; set; }
        public virtual DbSet<MVerfiyCode> MVerfiyCode { get; set; }

        public virtual DbSet<MGoal> MGoal { get; set; }
        public virtual DbSet<MUserAllergy> MUserAllergy { get; set; }
        public virtual DbSet<MUserGoal> MUserGoal { get; set; }
        public virtual DbSet<MUserRisk> MUserRisk { get; set; }
        public virtual DbSet<MMealType> MMealType { get; set; }
        public virtual DbSet<MFoodSteps> MFoodSteps { get; set; }
        public virtual DbSet<MIngredient> MIngredient { get; set; }

        public virtual DbSet<MMeal> MMeal { get; set; }
        public virtual DbSet<MDislikeMeal> MDislikeMeal { get; set; }
        public virtual DbSet<MMealIngredient> MMealIngredient { get; set; }
        public virtual DbSet<MUserMeal> MUserMeal { get; set; }
        public virtual DbSet<MMealSteps> MMealSteps { get; set; }

        public virtual DbSet<MVideo> MVideo { get; set; }
        public virtual DbSet<MBlogType> MBlogType { get; set; }
        public virtual DbSet<MMediaType> MMediaType { get; set; }
        public virtual DbSet<MAttachmentType> MAttachmentType { get; set; }

        public virtual DbSet<SecUser> SecUser { get; set; }
        public virtual DbSet<MSplash> MSplash { get; set; }

        #endregion
    }
}