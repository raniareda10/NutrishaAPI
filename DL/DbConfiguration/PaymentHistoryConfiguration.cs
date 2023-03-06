using DL.EntitiesV1.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DL.DbConfiguration
{
    public class PaymentHistoryConfiguration : IEntityTypeConfiguration<PaymentHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentHistoryEntity> builder)
        {
            builder.HasOne(m => m.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}