
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Brands;
using ProductService.Domain.Brands.ValueObjects;
namespace ProductService.Infrastructure.Persistence.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brand", schema: "Product");
            builder.HasKey(c => c.Id).HasName("PK_Brand");

            builder.Property(c => c.Id)
                .HasConversion(new BrandIdConversion())
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()")
                .HasColumnName($"BrandId");

            // -----------------------
            // Core Value Objects
            // -----------------------
            builder.Property(c => c.Name).HasConversion(v => v.Value, v => BrandName.Create(v)).HasColumnName("Name").HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Property(c => c.Code).HasConversion(v => v.Value, v => BrandCode.Create(v)).HasColumnName("Code").HasColumnType("varchar(20)").HasMaxLength(20).IsRequired();
            builder.Property(c => c.Slug).HasConversion(v => v.Value, v => BrandSlug.Create(v)).HasColumnName("Slug").HasColumnType("varchar(160)").HasMaxLength(160).IsRequired();
            builder.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").HasDefaultValue(true);

            // -----------------------
            // Audit fields
            builder.Property(c => c.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasColumnType("varchar(100)").HasDefaultValueSql("SUSER_NAME()").IsRequired();
            builder.Property(c => c.LastModifiedBy).HasMaxLength(100);
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit").HasDefaultValue(false);
            builder.Property(c => c.DeletedBy).HasMaxLength(100);

            // Indices
            builder.HasIndex(p => p.Name).IsUnique().HasDatabaseName("UQ_Brand_Name");
            builder.HasIndex(p => p.Code).IsUnique().HasDatabaseName("UQ_Brand_Code");
            builder.HasIndex(p => p.Slug).IsUnique().HasDatabaseName("UQ_Brand_Slug");
            
        }
    }
}
