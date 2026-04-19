namespace OrderService.Api.Constracts.Responses
{
    public sealed record OrderDetailResponse(
        Guid OrderId,
        string OrderNumber,
        string Status,
        string Currency,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal TaxtTotal,
        decimal Total,
        List<OrderItemResponse>? Items
    );
}
