using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class EmployeeMapping : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.User)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Address)
                .WithOne();

            builder.HasOne(d => d.BankData)
                .WithOne();

            builder.HasMany(d => d.DocumentPhotos)
                .WithOne(d => d.Employee)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Employees");
        }
    }
}
