

using MassTransit;

namespace Contracts.Commands
{
    [EntityName("event.order.created")]
    public record CreateOrderCommand(Guid OrderId, Guid EventId, string Provider, string Currency, List<TicketDto> Tickets, string CustomerId);

    public record TicketDto(Guid ZoneId, decimal Price);
}
