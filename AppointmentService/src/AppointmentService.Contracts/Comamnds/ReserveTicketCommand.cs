using OrderService.Contracts.Dtos;

namespace OrderService.Contracts.Comamnds
{
    public sealed record ReserveTicketCommand(
        Guid OrderId, 
        Guid EventId,
        IEnumerable<ZoneRequest> Zones
    );
}
