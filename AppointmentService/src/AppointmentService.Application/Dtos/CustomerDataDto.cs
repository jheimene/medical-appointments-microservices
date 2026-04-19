namespace OrderService.Application.Dtos
{
    public sealed record CustomerDataDto(
        Guid CustomerId,
        string Name,
        string LastName,
        string DocumentType,
        string DocumentNumber,
        string? Email,
        string? Phone,
        bool IsActive
    );
}
