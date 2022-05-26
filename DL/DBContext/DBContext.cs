using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Polls;
using DL.EntitiesV1.Comments;
using DL.EntitiesV1.Media;
using DL.EntitiesV1.Reactions;
using DL.EntityTypeBuilders;
using Newtonsoft.Json;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MUser>().HasIndex(p => new {  p.Mobile }).IsUnique();
            ConfigureBlogs(modelBuilder);
        }

        private void ConfigureBlogs(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property(b => b.Media)
                .HasConversion(
                    media => JsonConvert.SerializeObject(media, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }),
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
