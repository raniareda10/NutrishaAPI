using DL.Entities;
using DL.EntitiesV1.AdminUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DL.DbConfiguration
{
    public class ResetPasswordConfiguration : IEntityTypeConfiguration<ResetUserPasswordEntity>
    {
        public void Configure(EntityTypeBuilder<ResetUserPasswordEntity> builder)
        {
            builder.HasKey(m => m.Id);
        }
    }
}