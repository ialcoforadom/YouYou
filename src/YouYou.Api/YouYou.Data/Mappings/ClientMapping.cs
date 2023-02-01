using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class ClientMapping : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(f => f.Address)
                .WithOne();

            builder.HasOne(f => f.BankData)
                .WithOne();

            builder.HasOne(f => f.User)
                .WithOne();

            builder.ToTable("Clients");
        }
    }
}
