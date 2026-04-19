using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Products.Enums;
using ProductService.Domain.Products.ValueObjects;


namespace ProductService.Infrastructure.Persistence.Configurations
{
    internal class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImage", "Product");
            builder.HasKey(p => p.Id).HasName("PK_ProductImageId");
            builder.Property(p => p.Id)
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .HasColumnName($"ProductImageId");

            //builder.HasOne<Product>().WithMany().HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(i => i.ProductId).HasConversion(id => id.Value, value => new ProductId(value)).HasColumnName("ProductId");

            builder.Property(i => i.ObjectKey).HasMaxLength(500).IsRequired();
            builder.Property(i => i.OriginalFileName).HasMaxLength(255).HasColumnName("FileName").IsRequired();
            builder.Property(i => i.ContentType).HasMaxLength(100).IsRequired();
            builder.Property(i => i.SizeBytes).IsRequired();
            builder.Property(i => i.Status).HasConversion<int>().HasDefaultValue(ProductImageStatus.Active);

            builder.Property(i => i.Url).HasConversion(v => v.Value, v => ImageUrl.Create(v)).HasMaxLength(255).IsRequired();
            builder.Property(i => i.AltText).HasMaxLength(255);
            builder.Property(i => i.IsMain).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(i => i.SortOrder).HasColumnType("smallint").HasDefaultValue(0);

            builder.Property<DateTime>("CreatedAt").IsRequired().HasColumnType("datetime").HasDefaultValueSql("getdate()").ValueGeneratedOnAdd();
            builder.Property<string>("CreatedBy").IsRequired().HasMaxLength(100).HasDefaultValueSql("suser_name()");
            builder.Property<DateTime?>("LastModifiedAt").HasColumnType("datetime");
            builder.Property<string>("LastModifiedBy").HasMaxLength(100);

            builder.HasIndex("ProductId");
            builder.HasIndex(i => new { i.ProductId, i.IsMain });
            builder .HasIndex(i => new { i.ProductId, i.SortOrder });
        }
    }
}
