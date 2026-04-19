namespace OrderService.Api.Constracts.Responses
{
    public sealed record CreateOrderResponse(
        Guid OrderId,
        string OrderNumber,
        string Status
    );
}
