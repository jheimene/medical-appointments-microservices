using CustomerService.Application.Customers.Dtos;
using ErrorOr;
using MediatR;

namespace CustomerService.Application.Customers.Queries.GetByIdCustomer
{
    public sealed record GetByIdCustomerQuery(Guid CustomerId) : IRequest<ErrorOr<CustomerDto>>;
}
