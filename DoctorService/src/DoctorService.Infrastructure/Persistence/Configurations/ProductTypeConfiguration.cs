using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.ProductsTypes;
using ProductService.Domain.ProductsTypes.ValueObjects;

namespace ProductService.Infrastructure.Persistence.Configurations
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.ToTable("ProductType", schema: "Product");
            builder.HasKey(c => c.Id).HasName("PK_ProductType");

            builder.Property(c => c.Id)
                .HasConversion(new ProductTypeIdConversion())
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("ProductTypeId");

            // -----------------------
            // Core Value Objects
            // -----------------------
            builder.Property(c => c.Name).HasConversion(v => v.Value, v => ProductTypeName.Create(v)).HasColumnName("Name").HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Property(c => c.Code).HasConversion(v => v.Value, v => ProductTypeCode.Create(v)).HasColumnName("Code").HasColumnType("varchar(20)").HasMaxLength(20).IsRequired();
            builder.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").HasDefaultValue(true);

            // -----------------------
            // Audit fields
            builder.Property(c => c.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasColumnType("varchar(100)").HasDefaultValueSql("SUSER_NAME()").IsRequired();
            builder.Property(c => c.LastModifiedBy).HasMaxLength(100);
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit").HasDefaultValue(false);
            builder.Property(c => c.DeletedBy).HasMaxLength(100);

            // Indices
            builder.HasIndex(p => p.Name).IsUnique().HasDatabaseName("UQ_ProductType_Name");
            builder.HasIndex(p => p.Code).IsUnique().HasDatabaseName("UQ_ProductType_Code");
        }
    }
}
