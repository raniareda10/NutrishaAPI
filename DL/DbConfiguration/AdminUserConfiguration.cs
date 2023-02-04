using DL.Entities;
using DL.EntitiesV1.AdminUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DL.DbConfiguration
{
    public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUserEntity>
    {
        public void Configure(EntityTypeBuilder<AdminUserEntity> builder)
        {
            builder
                .HasIndex(m => m.Email)
                .IsUnique();

            builder.Property(m => m.Email)
                .IsRequired();

            builder.Property(m => m.Name)
                .HasMaxLength(320);
            
        }
    }
}