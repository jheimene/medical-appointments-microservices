
namespace OrderService.Contracts.Events
{
    public sealed record OrderFailedEvent(
        Guid OrderId,
        Guid EventId
    );
}
