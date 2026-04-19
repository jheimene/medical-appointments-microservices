using OrderService.Domain.Orders.Enums;

namespace OrderService.Domain.Orders
{
    public sealed record OrderSnapshot(
        Guid Id,
        Guid OrderId,
        string OrderNumber,
        Guid CustomerId,
        string CustomerName,
        OrderStatus Status,
        string Currency,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal TaxTotal,
        decimal Total,
        string? Notes,
        string? IdempotencyKey,
        IReadOnlyCollection<OrderItemSnapshot> Items,
        string CreatedBy,
        DateTime CreateAt,
        string LastModifiedBy,
        DateTime? LastModifiedAt

    );

    public sealed record OrderItemSnapshot(
        Guid Id,
        Guid OrderId,
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity,
        decimal DiscountAmount,
        decimal TaxAmount,
        decimal LineTotal
    );
}
