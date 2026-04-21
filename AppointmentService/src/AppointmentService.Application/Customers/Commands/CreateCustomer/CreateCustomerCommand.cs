using ErrorOr;
using MediatR;

namespace AppointmentService.Application.Customers.Commands.CreateCustomer
{
    public sealed record CreateCustomerCommand(
        Guid PatientId,
        Guid DoctorId,
        DateTime AppointmentDate,
        string Reason
    ) : IRequest<ErrorOr<Guid>>;
}