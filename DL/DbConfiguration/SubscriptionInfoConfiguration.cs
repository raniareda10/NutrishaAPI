using DL.EntitiesV1.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DL.DbConfiguration
{
    public class SubscriptionInfoConfiguration : IEntityTypeConfiguration<SubscriptionInfoEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptionInfoEntity> builder)
        {
            builder.HasOne(m => m.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}