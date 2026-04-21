using AppointmentService.Application.Customers.Dtos;
using AppointmentService.Domain.Interfaces;
using ErrorOr;
using MediatR;

namespace AppointmentService.Application.Customers.Queries.GetByIdCustomer
{
    public sealed class GetByIdCustomerQueryHandler : IRequestHandler<GetByIdCustomerQuery, ErrorOr<CustomerDto>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetByIdCustomerQueryHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<ErrorOr<CustomerDto>> Handle(GetByIdCustomerQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.CustomerId);
            if (appointment == null)
            {
                return Error.NotFound("Appointment.NotFound", $"Appointment with id {request.CustomerId} not found.");
            }

            return new CustomerDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Reason = appointment.Reason,
                Status = appointment.Status
            };
        }
    }
}