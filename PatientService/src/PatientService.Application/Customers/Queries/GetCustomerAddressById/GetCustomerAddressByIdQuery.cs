using MediatR;

namespace CustomerService.Application.Customers.Queries.GetCustomerAddressById
{
    public sealed record GetCustomerAddressByIdQuery(
        Guid CustomerId,
        Guid AddressId
    ) : IRequest<GetCustomerAddressByIdQueryResponse>
    {
    }
}
