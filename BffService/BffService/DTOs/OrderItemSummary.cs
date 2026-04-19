namespace BffService.DTOs
{
    public sealed record OrderItemSummary(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal
    );
}
