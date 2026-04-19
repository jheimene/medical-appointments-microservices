using OrderService.Contracts.Dtos;

namespace OrderService.Contracts.Events
{
    public sealed record TicketReservedEvent(
        Guid OrderId, 
        Guid EventId,
        Guid UserId,
        PaymentRequest Payment = default!
    );
}
