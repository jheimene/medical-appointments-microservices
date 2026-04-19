

namespace Contracts.Commands
{
    public record ReserveTicketCompesationCommand (Guid OrderId, Guid EventId, List<TicketDto> Tickets, int Quantity)
    {
    }
}
