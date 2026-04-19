namespace BffService.DTOs
{
    public sealed record OrderItemDto(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice
    );

}
