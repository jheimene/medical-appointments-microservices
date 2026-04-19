namespace OrderService.Api.Constracts.Requests
{
    public sealed record CreateOrderItemRequest(
        Guid ProductId,
        int Quantity,
        decimal Price
    );
}
