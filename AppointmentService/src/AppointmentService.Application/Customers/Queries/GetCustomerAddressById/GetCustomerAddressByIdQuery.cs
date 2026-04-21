using MediatR;

namespace AppointmentService.Application.Customers.Queries.GetCustomerAddressById
{
    public sealed record GetCustomerAddressByIdQuery(
        Guid CustomerId,
        Guid AddressId
    ) : IRequest<GetCustomerAddressByIdQueryResponse>
    {
    }
}
