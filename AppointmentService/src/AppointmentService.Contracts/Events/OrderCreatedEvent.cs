
using OrderService.Contracts.Dtos;

namespace OrderService.Contracts.Events
{
    public sealed record OrderCreatedEvent(
        Guid OrderId, 
        Guid CustomerId,
        //Guid UserId,
        PaymentRequest Payment = default!,
        IEnumerable<ZoneRequest> Zonas = default!
    );

}
