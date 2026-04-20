
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorService.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer", schema: "Customer");

            builder.HasKey(c => c.Id).HasName("PK_Customer");

            builder.Property(c => c.Id)
                .HasConversion(new CustomerIdConversion())
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .HasColumnName($"CustomerId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(80);

            builder.Property(c => c.LastName).IsRequired().HasColumnType("varchar(80)");

            builder.OwnsOne(c => c.Document, dr =>
            {
                dr.Property(d => d.Type).IsRequired().HasConversion<string>().HasColumnName("DocumentType").HasColumnType("varchar(12)");
                dr.Property(d => d.Number).IsRequired().HasColumnName("DocumentNumber").HasColumnType("varchar(20)");
            });

            builder.Property(c => c.Email).HasConversion(v => v!.Address, v => Email.Create(v)).HasColumnName("Email").HasColumnType("varchar(100)");

            builder.Property(c => c.Phone).HasConversion(v => v!.Number, v => new PhoneNumber(v)).HasColumnName("PhoneNumber").HasColumnType("varchar(50)");

            builder.Property(c => c.Status).IsRequired().HasConversion<string>().HasColumnType("varchar(20)"); //.HasDefaultValue(EventStatus.Draft);

            builder.HasMany(c => c.Address)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.BirthDate).HasColumnType("date").HasColumnName("BirthDate");

            builder.Property(c => c.Gender).HasConversion<string>().HasColumnType("varchar(10)").HasColumnName("Gender");

        }
    }
}
