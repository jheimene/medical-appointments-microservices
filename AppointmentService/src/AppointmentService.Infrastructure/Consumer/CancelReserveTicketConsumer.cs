using MassTransit;
using OrderService.Contracts.Events;

namespace OrderService.Infrastructure.Consumer
{
    public sealed class CancelReserveTicketConsumer : IConsumer<PaymentFailedEvent>
    {
        public Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
