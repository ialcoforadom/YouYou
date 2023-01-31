using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.NickName).IsRequired(false)
                .HasColumnType("varchar(256)");

            builder.Property(c => c.IsCompany)
                .HasDefaultValue(false)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(c => c.Disabled)
                .HasDefaultValue(false)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired()
                .HasColumnType("bit");

            builder.HasMany(f => f.ExtraPhones)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder.HasMany(f => f.UserRoles)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.PhysicalPerson)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.JuridicalPerson)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
