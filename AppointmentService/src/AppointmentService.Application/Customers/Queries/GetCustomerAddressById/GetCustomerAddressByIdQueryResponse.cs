namespace AppointmentService.Application.Customers.Queries.GetCustomerAddressById
{
    public record class GetCustomerAddressByIdQueryResponse(
        Guid CustomerId,
        Guid CustomerAddressId,
        string Street,
        string District,
        string Province,
        string Departament,
        string? Reference,
        string Label,
        bool IsDefault
        )
    {
    }
}
