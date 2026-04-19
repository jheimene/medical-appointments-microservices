namespace OrderService.Api.Constracts.Requests
{
    public sealed record CreateOrderRequest(
        Guid CustomerId,
        string Currency,
        string CreatedBy,
        string? Notes,
        string? Provider,
        IReadOnlyCollection<CreateOrderItemRequest> Items
    );
}
