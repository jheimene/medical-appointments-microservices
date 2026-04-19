namespace BffService.DTOs
{
    public sealed record OrderSummaryResponse(
        Guid OrderId,
        string OrderNumber,
        DateTime CreatedAt,
        CustomerSummary Customer,
        PaymentSummary Payment,
        ShipmentSummary Shipment,
        IReadOnlyList<OrderItemSummary> Items,
        decimal Total
    );
}
