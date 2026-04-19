
using OrderService.Contracts.Dtos;

namespace OrderService.Contracts.Comamnds
{
    public sealed record ReleaseTicketCommand(
        Guid OrderId,
        Guid EventId,
        IEnumerable<ZoneRequest> Zones
    );
}
