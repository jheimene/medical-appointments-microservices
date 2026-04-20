using MediatR;

namespace DoctorService.Application.Customers.Queries.GetCustomerAddressById
{
    public sealed record GetCustomerAddressByIdQuery(
        Guid CustomerId,
        Guid AddressId
    ) : IRequest<GetCustomerAddressByIdQueryResponse>
    {
    }
}
