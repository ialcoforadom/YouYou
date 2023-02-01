using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class TypeGenderMapping : IEntityTypeConfiguration<TypeGender>
    {
        public void Configure(EntityTypeBuilder<TypeGender> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(256)");

            builder.ToTable("TypeGenders");
        }
    }
}