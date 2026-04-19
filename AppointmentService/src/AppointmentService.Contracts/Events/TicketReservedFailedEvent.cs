namespace OrderService.Contracts.Events
{
    public sealed record TicketReservedFailedEvent(Guid OrderId, Guid EventId);
}
