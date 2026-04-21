using AppointmentService.Application.Customers.Dtos;
using AppointmentService.Domain.Enums;
using ErrorOr;
using MediatR;

namespace AppointmentService.Application.Customers.Queries.GetByIdCustomer
{
    public sealed class GetByIdCustomerQueryHandler : IRequestHandler<GetByIdCustomerQuery, ErrorOr<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetByIdCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public async Task<ErrorOr<CustomerDto>> Handle(GetByIdCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));
            if (customer == null)
            {
                return Error.NotFound("Appointment.NotFound", $"Appointment with id {request.CustomerId} not found.");
            }

            return new CustomerDto
            {
                AppointmentId = customer!.Id.Value,
                PatientId = Guid.Empty,
                DoctorId = Guid.Empty,
                AppointmentDate = null,
                Reason = customer.Name,
                Status = customer.Status.ToString()
            };
        }
    }
}