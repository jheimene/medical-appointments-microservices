
namespace Contracts.Commands
{
    public record ReserveTicketCommand(Guid OrderId, Guid EventId, List<TicketDto> Tickets, int Quantity);
}
