using OrderService.Contracts.Dtos;

namespace OrderService.Contracts.Comamnds
{
    public sealed record InitializeOrderCommand (
        Guid OrderId, 
        Guid EventId,
        string CustomerId,
        PaymentRequest Payment,
        IEnumerable<ZoneRequest> Zones
    );
}
