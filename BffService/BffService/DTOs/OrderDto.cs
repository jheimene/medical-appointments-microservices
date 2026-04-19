namespace BffService.DTOs
{
    public sealed record OrderDto(
        Guid Id,
        string OrderNumber,
        DateTime CreatedAt,
        Guid CustomerId,
        Guid PaymentId,
        Guid ShipmentId,
        decimal Total,
        IReadOnlyList<OrderItemDto> Items
    );
}
