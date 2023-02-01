using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class PhysicalPersonMapping : IEntityTypeConfiguration<PhysicalPerson>
    {
        public void Configure(EntityTypeBuilder<PhysicalPerson> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CPF)
                .IsRequired()
                .HasColumnType("varchar(11)");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(256)");

            builder.Property(d => d.Birthday)
                .IsRequired(false)
                .HasColumnType("datetime");

            builder.HasOne(d => d.Gender)
                .WithOne();

            builder.ToTable("PhysicalPersons");
        }
    }
}
