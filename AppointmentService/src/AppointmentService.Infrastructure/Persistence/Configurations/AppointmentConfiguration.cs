using AppointmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentService.Infrastructure.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointment");

            builder.HasKey(x => x.AppointmentId);

            builder.Property(x => x.AppointmentId)
                .IsRequired();

            builder.Property(x => x.PatientId)
                .IsRequired();

            builder.Property(x => x.DoctorId)
                .IsRequired();

            builder.Property(x => x.AppointmentDate)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreatedAt);
            builder.Property(x => x.CreatedBy)
                .HasMaxLength(255);
        }
    }
}