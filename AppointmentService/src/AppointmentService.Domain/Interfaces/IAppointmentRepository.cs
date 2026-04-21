using AppointmentService.Domain.Entities;

namespace AppointmentService.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task CreateAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(Guid appointmentId);
    }
}