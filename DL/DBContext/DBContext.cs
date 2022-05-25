using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DBContext
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MUser>().HasIndex(p => new {  p.Mobile }).IsUnique();
        }
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

        public virtual DbSet<MArticle> MArticle { get; set; }
        public virtual DbSet<MArticleComment> MArticleComment { get; set; }
        public virtual DbSet<MArticleLike> MArticleLike { get; set; }
        public virtual DbSet<MArticleCommentLike> MArticleCommentLike { get; set; }
        public virtual DbSet<MPoll> MPoll { get; set; }
        public virtual DbSet<MVideo> MVideo { get; set; }
        public virtual DbSet<MBlogType> MBlogType { get; set; }
        public virtual DbSet<MMediaType> MMediaType { get; set; }
        public virtual DbSet<MArticleAttachment> MArticleAttachment { get; set; }
        public virtual DbSet<MAttachmentType> MAttachmentType { get; set; }
        public virtual DbSet<MPollAnswer> MPollAnswer { get; set; }

        public virtual DbSet<MUserPollAnswer> MUserPollAnswer { get; set; }
        public virtual DbSet<SecUser> SecUser { get; set; }
        public virtual DbSet<MSplash> MSplash { get; set; }

    }
}
