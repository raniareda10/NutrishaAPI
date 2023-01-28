using DL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DL.DbConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<MUser>
    {
        public void Configure(EntityTypeBuilder<MUser> builder)
        {
            // builder.HasData();
        }
    }
}