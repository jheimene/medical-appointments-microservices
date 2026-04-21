namespace AppointmentService.Application.Abstractions.Clients
{
    public interface IDoctorServiceClient
    {
        Task<bool> DoctorExistsAsync(Guid doctorId);
    }
}