namespace OrderService.Contracts.Events
{
    public sealed record PaymentProcessedEvent (
        Guid OrderId,
        Guid EventId
    );
}
