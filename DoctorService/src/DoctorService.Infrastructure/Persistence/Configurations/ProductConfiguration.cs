
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Brands;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;
using ProductService.Domain.Products.Enums;
using ProductService.Domain.Products.ValueObjects;
using ProductService.Domain.ProductsTypes.ValueObjects;

namespace ProductService.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        private const string TableName = "Product";
        private const string SchemaName = "Product";
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            var allowedNames = string.Join(", ",
                  Enum.GetNames<ProductStatus>()
                      .Select(name => $"'{name}'"));

            builder.ToTable($"{TableName}", schema: $"{SchemaName}", table => {
                table.HasCheckConstraint("CHK_Product_Status", $"[Status] IN ({allowedNames})");
            });

            // -----------------------
            // Key (ProductId)
            // -----------------------
            builder.HasKey(c => c.Id).HasName("PK_Product");

            builder.Property(c => c.Id)
                .HasConversion(new ProductIdConversion())
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .HasColumnName($"ProductId");

            // -----------------------
            // Core Value Objects
            // -----------------------
            builder.Property(c => c.Name).HasConversion(v => v.Value, v => ProductName.Create(v)).HasColumnName("Name").HasColumnType("varchar(120)").HasMaxLength(100).IsRequired();
            builder.Property(c => c.Slug).HasConversion(v => v.Value, v => Slug.Create(v)).HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            builder.Property(c => c.Sku).HasConversion(v => v.Value, v => Sku.Create(v)).HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            builder.Property(c => c.Description).HasColumnName("Description").HasMaxLength(2500);
            builder.Property(c => c.Status).HasConversion<string>().HasColumnType("varchar(20)").HasMaxLength(20).HasDefaultValue(ProductStatus.Draf).IsRequired();
            builder.Property(c => c.ProductTypeId).HasConversion(id => id.Value, value => new ProductTypeId(value)).HasColumnName("ProductTypeId").IsRequired();
            builder.Property(c => c.BrandId).HasConversion(id => id.Value, value => new BrandId(value)).HasColumnName("BrandId").IsRequired();
            builder.OwnsOne(p => p.Model, nb => nb.Property(n => n.Value).HasColumnName("Model").HasMaxLength(60).IsRequired(false));

            builder.HasOne<Brand>().WithMany().HasForeignKey(p => p.BrandId).OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(p => p.Price, dr => {
                dr.Property(c => c.Currency).HasConversion(v => v.Code, v => Currency.Create(v)).HasColumnName("Currency").HasColumnType("char(3)").IsRequired();
                dr.Property(p => p.Amount).HasColumnName("Price").HasColumnType("decimal(18,2)").IsRequired().HasDefaultValue(0);
            });

            builder.OwnsMany(p => p.CategoryIds, a => {
                a.ToTable("ProductCategory", "Product");

                a.WithOwner().HasForeignKey("ProductId");
                a.Property<ProductId>("ProductId").HasConversion(id => id.Value, value => new ProductId(value)).HasColumnName("ProductId").HasColumnOrder(1);

                a.Property(c => c.CategoryId).HasConversion(id => id.Value, value => new CategoryId(value)).HasColumnName("CategoryId").IsRequired();
                a.HasOne<Category>().WithMany().HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Cascade);

                a.Property<DateTime>("CreatedAt").IsRequired().HasColumnType("datetime").HasDefaultValueSql("getdate()").ValueGeneratedOnAdd();
                a.Property<string>("CreatedBy").IsRequired().HasMaxLength(100).HasDefaultValueSql("suser_name()");

                //.HasPrincipalKey(c => c.Id)
                //.HasConstraintName("FK_ProductCategory_CategoryId");

                a.HasKey("ProductId", "CategoryId").HasName("PK_ProductCategoryId");
                a.HasIndex("CategoryId");
            });

            /*
            builder.OwnsMany(c => c.Images, a =>
            {
                a.ToTable("ProductImage", "Product");
                a.WithOwner().HasForeignKey("ProductId");
                a.Property<ProductId>("ProductId").HasConversion(id => id.Value, value => new ProductId(value)).HasColumnName("ProductId").HasColumnOrder(2);

                a.Property<Guid>("ProductImageId").ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()").HasColumnOrder(1);
                a.HasKey("ProductImageId").HasName("PK_ProductImageId");

                a.Property(i => i.Url).HasConversion(v => v.Value, v => ImageUrl.Create(v)).HasMaxLength(255).IsRequired();
                a.Property(i => i.AltText).HasMaxLength(255);
                a.Property(i => i.IsMain).HasColumnType("bit").HasDefaultValue(false);
                a.Property(i => i.SortOrder).HasColumnType("smallint").HasDefaultValue(0);

                // ✅ Auditoría (shadow)
                a.Property<DateTime>("CreatedAt").IsRequired().HasColumnType("datetime").HasDefaultValueSql("getdate()").ValueGeneratedOnAdd();
                a.Property<string>("CreatedBy").IsRequired().HasMaxLength(100).HasDefaultValueSql("suser_name()");
                a.Property<DateTime?>("LastModifiedAt").HasColumnType("datetime");
                a.Property<string>("LastModifiedBy").HasMaxLength(100);
                a.HasIndex("ProductId");
            });
            */

            builder.HasMany(x => x.Images).WithOne().HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.Cascade);

            builder.OwnsMany(c => c.Tags, a =>
            {
                a.ToTable("ProductTag", "Product");
                a.WithOwner().HasForeignKey("ProductId");
                a.Property<ProductId>("ProductId").HasConversion(id => id.Value, value => new ProductId(value)).HasColumnName("ProductId").HasColumnOrder(2);

                a.Property<Guid>("ProductTagId").ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()").HasColumnOrder(1);
                a.HasKey("ProductTagId").HasName("PK_ProductTagId");

                a.Property(t => t.Value).HasColumnName("Tag").HasMaxLength(50).IsRequired();
                a.Property<DateTime>("CreatedAt").IsRequired().HasColumnType("datetime").HasDefaultValueSql("getdate()").ValueGeneratedOnAdd();
                a.Property<string>("CreatedBy").IsRequired().HasMaxLength(100).HasDefaultValueSql("suser_name()");
                a.Property<DateTime?>("LastModifiedAt").HasColumnType("datetime");
                a.Property<string>("LastModifiedBy").HasMaxLength(100);

                a.HasIndex("ProductId");
            });

            builder.OwnsMany(c => c.Attributes, a =>
            {
                a.ToTable("ProductAttribute", "Product");
                a.WithOwner().HasForeignKey("ProductId");
                a.Property<ProductId>("ProductId").HasConversion(id => id.Value, value => new ProductId(value)).HasColumnName("ProductId").HasColumnOrder(2);

                a.Property<Guid>("ProductAttributeId").ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()").HasColumnOrder(1);
                a.HasKey("ProductAttributeId").HasName("PK_ProductAttributeId");

                a.Property(t => t.Key).HasConversion(id => id.Value, value => AttributeKey.Create(value)).HasMaxLength(50).IsRequired();
                a.Property(t => t.Value).HasConversion(id => id.Value, value => AttributeValue.Create(value)).HasMaxLength(150).IsRequired();
                a.Property(t => t.IsFilterable).HasColumnType("bit").HasDefaultValue(false);

                a.Property<DateTime>("CreatedAt").IsRequired().HasColumnType("datetime").HasDefaultValueSql("getdate()").ValueGeneratedOnAdd();
                a.Property<string>("CreatedBy").IsRequired().HasMaxLength(100).HasDefaultValueSql("suser_name()");
                a.Property<DateTime?>("LastModifiedAt").HasColumnType("datetime");
                a.Property<string>("LastModifiedBy").HasMaxLength(100);

                a.HasIndex("ProductId");
            });

            // -----------------------
            // Audit fields
            builder.Property(c => c.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasColumnType("varchar(100)").HasDefaultValueSql("SUSER_NAME()").IsRequired();
            builder.Property(c => c.LastModifiedBy).HasMaxLength(100);
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit").HasDefaultValue(false);
            builder.Property(c => c.DeletedBy).HasMaxLength(100);

            // Indices
            builder.HasIndex(p => p.Name).IsUnique().HasDatabaseName("UQ_Product_Name");
            builder.HasIndex(p => p.Slug).IsUnique().HasDatabaseName("UQ_Product_Slug");
            builder.HasIndex(p => p.Sku).IsUnique().HasDatabaseName("UQ_Product_Sku");

            builder.HasIndex(c => c.BrandId).HasDatabaseName("IX_Product_BrandId");

        }
    }
}
