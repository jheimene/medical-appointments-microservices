namespace AppointmentService.Application.Customers.Dtos
{
    public sealed record CustomerDto
    {
        public Guid AppointmentId { get; init; }
        public Guid PatientId { get; init; }
        public Guid DoctorId { get; init; }
        public DateTime? AppointmentDate { get; init; }
        public string? Reason { get; init; }
        public string Status { get; init; } = default!;
    }
}