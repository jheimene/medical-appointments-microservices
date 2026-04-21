namespace AppointmentService.Domain.Entities
{
    public sealed class Appointment
    {
        public Guid AppointmentId { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid DoctorId { get; private set; }
        public DateTime AppointmentDate { get; private set; }
        public string Reason { get; private set; } = default!;
        public string Status { get; private set; } = "Scheduled";
        public DateTime? CreatedAt { get; private set; }
        public string? CreatedBy { get; private set; }

        private Appointment() { }

        public static Appointment Create(
            Guid patientId,
            Guid doctorId,
            DateTime appointmentDate,
            string reason)
        {
            return new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = appointmentDate,
                Reason = reason,
                Status = "Scheduled",
                CreatedAt = DateTime.Now,
                CreatedBy = "SISTEMA"
            };
        }
    }
}