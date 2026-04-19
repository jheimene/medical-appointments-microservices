using MassTransit;
using OrderService.Contracts.Events;

namespace OrderService.Infrastructure.Consumer
{
    public sealed class ConfirmReserveTicketConsumer : IConsumer<PaymentProcessedEvent>
    {
        public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            Task.Run(() => Console.WriteLine(context.Message));

            return Task.CompletedTask;
            // Actualizar el estado de las reservas de tickets asociadas a la orden
        }
    }
}
