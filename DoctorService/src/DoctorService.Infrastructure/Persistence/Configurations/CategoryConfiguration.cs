

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Cat.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;

namespace ProductService.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category", schema: "Product");
            builder.HasKey(c => c.Id).HasName("PK_Category");

            builder.Property(c => c.Id)
                .HasConversion(new CategoryIdConversion())
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .HasColumnName($"CategoryId");

            // -----------------------
            // Core Value Objects
            // -----------------------
            builder.Property(c => c.Name).HasConversion(v => v.Value, v => CategoryName.Create(v)).HasColumnName("Name").HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Property(c => c.Code).HasConversion(v => v.Value, v => CategoryCode.Create(v)).HasColumnName("Code").HasColumnType("varchar(20)").HasMaxLength(20).IsRequired();
            builder.Property(c => c.Slug).HasConversion(v => v.Value, v => CategorySlug.Create(v)).HasColumnName("Slug").HasColumnType("varchar(160)").HasMaxLength(220).IsRequired();
            builder.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").HasDefaultValue(true);
            
            builder.Property(c => c.ParentId)
                .HasConversion(new CategoryIdConversion())
                .HasColumnType("uniqueidentifier")
                .HasColumnName($"CategoryParentId")
                .IsRequired(false);

             builder.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .HasPrincipalKey(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict);

            // -----------------------
            // Audit fields
            builder.Property(c => c.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasColumnType("varchar(100)").HasDefaultValueSql("SUSER_NAME()").IsRequired();
            builder.Property(c => c.LastModifiedBy).HasMaxLength(100);
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit").HasDefaultValue(false);
            builder.Property(c => c.DeletedBy).HasMaxLength(100);

            // Indices
            builder.HasIndex(c => c.ParentId); 
            builder.HasIndex(p => p.Name).IsUnique().HasDatabaseName("UQ_Category_Name");
            builder.HasIndex(p => p.Code).IsUnique().HasDatabaseName("UQ_Category_Code");
            builder.HasIndex(p => p.Slug).IsUnique().HasDatabaseName("UQ_Category_Slug");
        }
    }
}
