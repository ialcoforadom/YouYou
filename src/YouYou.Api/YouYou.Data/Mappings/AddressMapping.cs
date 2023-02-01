using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CEP)
                .HasColumnType("varchar(8)");

            builder.Property(c => c.Street)
                .IsRequired()
                .HasColumnType("varchar(256)");

            builder.Property(c => c.Number)
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Neighborhood)
                .HasColumnType("varchar(256)");

            builder.Property(c => c.Complement)
                .HasColumnType("varchar(256)");

            builder.HasOne(f => f.City);

            builder.ToTable("Addresses");
        }
    }
}
