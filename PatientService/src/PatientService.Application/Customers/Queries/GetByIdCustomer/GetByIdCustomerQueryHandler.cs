using CustomerService.Application.Customers.Dtos;
using CustomerService.Domain.Enums;
using ErrorOr;
using MediatR;

namespace CustomerService.Application.Customers.Queries.GetByIdCustomer
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
                CustomerId = customer!.Id.Value,
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
