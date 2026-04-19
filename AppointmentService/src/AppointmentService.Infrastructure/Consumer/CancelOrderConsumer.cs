using MassTransit;
using OrderService.Contracts.Events;

namespace OrderService.Infrastructure.Consumer
{
    public class CancelOrderConsumer : IConsumer<PaymentFailedEvent>
    {
        public Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
