
namespace Contracts.Commands
{
    public record ProcessPaymentCommand(Guid OrderId, Guid EventId, string Provider, string Currency, decimal Amount, string UserId);
}
