using MediatR;

namespace DoctorService.Application.Customers.Queries.GetCustomerAddressById
{
    public sealed class GetCustomerAddressByIdQueryHandler : IRequestHandler<GetCustomerAddressByIdQuery, GetCustomerAddressByIdQueryResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerAddressByIdQueryHandler(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public async Task<GetCustomerAddressByIdQueryResponse> Handle(GetCustomerAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));

            if (customer is null)
            {
                throw new KeyNotFoundException($"Customer with id {request.CustomerId} not found.");
            }   

            var customerAddress = customer.Address.FirstOrDefault(a => a.Id.Value == request.AddressId);
            if (customerAddress is null) {
                throw new KeyNotFoundException($"Address with id {request.AddressId} not found for customer with id {request.CustomerId}.");
            }

            return new GetCustomerAddressByIdQueryResponse(
            
                CustomerId: customer.Id.Value,
                CustomerAddressId: customerAddress.Id.Value,
                Label: customerAddress.Label,
                Street: customerAddress.Address.Street,
                District: customerAddress.Address.District,
                Province: customerAddress.Address.Province,
                Departament: customerAddress.Address.Departament,
                Reference: customerAddress.Address.Reference,
                IsDefault: customerAddress.IsDefault
            );
        }
    }
}
