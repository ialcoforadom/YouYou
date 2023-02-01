using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class BankDataMapping : IEntityTypeConfiguration<BankData>
    {
        public void Configure(EntityTypeBuilder<BankData> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.BankName)
                .IsRequired()
                .HasColumnType("varchar(256)");

            builder.Property(c => c.Agency)
                .IsRequired()
                .HasColumnType("varchar(5)");

            builder.Property(c => c.Account)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(c => c.CpfOrCnpjHolder)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.Property(c => c.IsHolder)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(c => c.PixKey)
                .HasColumnType("varchar(32)");

            builder.ToTable("BankData");
        }
    }
}
