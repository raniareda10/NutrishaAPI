using DL.Entities;
using DL.EntitiesV1.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DL.DbConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasOne(m => m.Role)
                .WithMany();
            
            builder.HasOne(m => m.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(r => r.RoleId);

            builder.HasOne(m => m.Permission)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(r => r.PermissionId);
        }
    }
}