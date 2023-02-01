using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class JuridicalPersonMapping : IEntityTypeConfiguration<JuridicalPerson>
    {
        public void Configure(EntityTypeBuilder<JuridicalPerson> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CNPJ)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.Property(c => c.CompanyName)
                .IsRequired()
                .HasColumnType("varchar(256)");

            builder.ToTable("JuridicalPersons");
        }
    }
}
