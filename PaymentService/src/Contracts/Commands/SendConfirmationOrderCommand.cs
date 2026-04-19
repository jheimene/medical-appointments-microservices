namespace Contracts.Commands
{
    public record SendConfirmationOrderCommand(Guid OrderId, string User) { }
}
