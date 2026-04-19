namespace OrderService.Contracts.Events
{
    public sealed record PaymentFailedEvent(
        Guid OrderId,
        Guid EventId
    );
}
