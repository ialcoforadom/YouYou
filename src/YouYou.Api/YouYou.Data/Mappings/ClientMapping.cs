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
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.User)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Clients");
        }
    }
}
