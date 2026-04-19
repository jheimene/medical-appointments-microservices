using ErrorOr;
using MediatR;

namespace CustomerService.Application.Customers.Commands.CreateCustomer
{
    public sealed record CreateCustomerCommand(
        string Name,
        string LastName,
        //IdentityDocumentType DocumentType,
        string DocumentType,
        string DocumentNumber,
        string? Email,
        string? PhoneNumber,
        DateOnly? BirthDate,
        Gender Gender
    ) : IRequest<ErrorOr<Guid>>;

}
