using AppointmentService.Application.Customers.Dtos;
using ErrorOr;
using MediatR;

namespace AppointmentService.Application.Customers.Queries.GetByIdCustomer
{
    public sealed record GetByIdCustomerQuery(Guid CustomerId) : IRequest<ErrorOr<CustomerDto>>;
}
