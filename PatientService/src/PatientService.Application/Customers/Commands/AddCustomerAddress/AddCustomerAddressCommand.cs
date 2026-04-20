using MediatR;

namespace PatientService.Application.Customers.Commands.AddCustomerAddress
{
    public sealed record AddCustomerAddressCommand (
        Guid CustomerId,
        string Street,
        string District,
        string Province,
        string Departament,
        string Reference,
        string Label,
        bool IsDefault
    ) : IRequest<Guid>
    {
    }
}
