using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PatientService.Infrastructure.Persistence.Configurations
{
    public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.ToTable("PatientAddress", schema: "Patient");

            builder.HasKey(c => c.Id).HasName("PK_PatientAddress");

            builder.Property(c => c.Id)
                .HasConversion(new CustomerAddressIdConversion())
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .HasColumnName("PatientAddressId");

            builder.Property(c => c.CustomerId)
                .HasConversion(new CustomerIdConversion())
                .HasColumnType("uniqueidentifier")
                .HasColumnName("PatientId");

            builder.Property(c => c.Label).IsRequired().HasColumnName("Label").HasMaxLength(30);

            builder.OwnsOne(c => c.Address, a =>
            {
                a.Property(p => p.Street).IsRequired().HasColumnName("Street").HasMaxLength(100);
                a.Property(p => p.District).IsRequired().HasColumnName("District").HasMaxLength(50);
                a.Property(p => p.Province).IsRequired().HasColumnName("Province").HasMaxLength(50);
                a.Property(p => p.Departament).IsRequired().HasColumnName("Departament").HasMaxLength(20);
                a.Property(p => p.Reference).HasColumnName("Reference").HasMaxLength(50);
            });

            builder.HasOne(c => c.Customer)
               .WithMany(c => c.Address)
               .HasForeignKey(c => c.CustomerId)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK_PatientAddress_Patient");
        }
    }
}