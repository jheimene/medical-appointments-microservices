

namespace Contracts.Events
{
    public record OrderFailedEvent(Guid OrderId, string Reason);
}
