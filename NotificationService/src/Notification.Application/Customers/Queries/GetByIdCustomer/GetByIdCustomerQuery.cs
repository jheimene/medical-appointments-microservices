using DispatchService.Application.Customers.Dtos;
using ErrorOr;
using MediatR;

namespace DispatchService.Application.Customers.Queries.GetByIdCustomer
{
    public sealed record GetByIdCustomerQuery(Guid CustomerId) : IRequest<ErrorOr<CustomerDto>>;
}
