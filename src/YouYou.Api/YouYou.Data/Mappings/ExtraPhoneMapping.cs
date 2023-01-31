using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class ExtraPhoneMapping : IEntityTypeConfiguration<ExtraPhone>
    {
        public void Configure(EntityTypeBuilder<ExtraPhone> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Number)
                .IsRequired()
                .HasColumnType("varchar(13)");

            builder.HasOne(f => f.User);

            builder.ToTable("ExtraPhones");
        }
    }
}
