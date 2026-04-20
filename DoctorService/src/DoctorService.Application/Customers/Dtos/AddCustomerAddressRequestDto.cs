namespace DoctorService.Application.Customers.Dtos
{
    public sealed record AddCustomerAddressRequestDto(
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
