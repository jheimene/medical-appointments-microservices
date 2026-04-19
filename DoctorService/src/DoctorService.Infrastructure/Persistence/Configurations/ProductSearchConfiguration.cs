
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductService.Infrastructure.Persistence.Configurations
{
    public class ProductSearchConfiguration : IEntityTypeConfiguration<ProductSearch>
    {
        public void Configure(EntityTypeBuilder<ProductSearch> builder)
        {
            builder.ToTable("ProductSearch", "Product");

            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.NameNormalized)
                .IsRequired()
                .HasMaxLength(200);
            builder.Ignore(p => p.NameNormalized);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.BrandId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Brand)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Model)
                .HasMaxLength(100);

            builder.Property(x => x.ModelNormalized)
                .HasMaxLength(100);
            builder.Ignore(p => p.ModelNormalized);

            builder.Property(x => x.Tags)
                .HasMaxLength(1000);

            builder.Property(x => x.Categories)
                .HasMaxLength(1000);

            builder.Property(x => x.Attributes)
                .HasMaxLength(2000);

            builder.Property(x => x.SearchDocument)
                .HasMaxLength(-1);

            builder.Property(x => x.SearchDocumentNormalized)
                .HasMaxLength(-1);

            builder.Property (x => x.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(x => x.Name);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
