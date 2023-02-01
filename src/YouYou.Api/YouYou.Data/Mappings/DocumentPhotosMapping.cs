using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouYou.Business.Models;

namespace YouYou.Data.Mappings
{
    public class DocumentPhotosMapping : IEntityTypeConfiguration<DocumentPhoto>
    {
        public void Configure(EntityTypeBuilder<DocumentPhoto> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(256)");

            builder.Property(c => c.FileType)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.DataFiles)
                .IsRequired()
                .HasColumnType("LONGBLOB");

            builder.ToTable("DocumentPhotos");
        }
    }
}