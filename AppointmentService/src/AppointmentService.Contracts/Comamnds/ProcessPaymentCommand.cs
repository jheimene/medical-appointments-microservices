using OrderService.Contracts.Dtos;

namespace OrderService.Contracts.Comamnds
{
    public sealed record ProcessPaymentCommand(
        Guid OrderId, 
        Guid EventId, 
        string CustomerId, 
        PaymentRequest Payment
    );
}
