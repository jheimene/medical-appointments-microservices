
namespace OrderService.Contracts.Events
{
    public sealed record OrderCompletedEvent(
        Guid OrderId,
        Guid EventId
    );
}
