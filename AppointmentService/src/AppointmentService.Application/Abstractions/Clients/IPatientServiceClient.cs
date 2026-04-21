namespace AppointmentService.Application.Abstractions.Clients
{
    public interface IPatientServiceClient
    {
        Task<bool> PatientExistsAsync(Guid patientId);
    }
}