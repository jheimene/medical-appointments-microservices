namespace Contracts.Events
{
    public record TicketReservedEvent(Guid OrderId, DateTime ReservedAt);
}
