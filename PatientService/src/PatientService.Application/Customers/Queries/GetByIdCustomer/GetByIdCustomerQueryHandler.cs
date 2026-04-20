using PatientService.Application.Customers.Dtos;
using PatientService.Domain.Enums;
using ErrorOr;
using MediatR;

namespace PatientService.Application.Customers.Queries.GetByIdCustomer
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
            if (customer == null) {
                //throw new NotFoundException(code: "customer.not_found", message: $"No existe un customer con id {request.CustomerId}.");
                return Error.NotFound("Customer.NotFound", $"Customer with id {request.CustomerId} not found.");
            }

            return new CustomerDto
            {
                PatientId = customer!.Id.Value,
                Name = customer.Name,
                LastName = customer.LastName,
                DocumentType = customer.Document.Type.ToString(),
                DocumentNumber = customer.Document.Number,
                Email = customer.Email?.Address ?? string.Empty,
                IsActive = (customer.Status == CustomerStatus.Active)
            };
        }
    }
}
