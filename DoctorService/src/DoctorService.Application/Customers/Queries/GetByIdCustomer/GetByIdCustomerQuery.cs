using DoctorService.Application.Customers.Dtos;
using ErrorOr;
using MediatR;

namespace DoctorService.Application.Customers.Queries.GetByIdCustomer
{
    public sealed record GetByIdCustomerQuery(Guid CustomerId) : IRequest<ErrorOr<CustomerDto>>;
}
