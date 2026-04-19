

namespace OrderService.Contracts.Comamnds
{
    public sealed record CancelOrderCommand(
        Guid OrderId,
        Guid EventId
    );
}
