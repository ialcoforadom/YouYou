using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class GenderMapping : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description)
                .IsRequired(false)
                .HasColumnType("varchar(256)");

            builder.HasOne(c => c.TypeGender);

            builder.ToTable("Genders");
        }
    }
}