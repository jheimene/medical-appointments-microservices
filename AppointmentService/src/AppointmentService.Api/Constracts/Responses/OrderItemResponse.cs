namespace OrderService.Api.Constracts.Responses
{
    public sealed record OrderItemResponse(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal
    );
}