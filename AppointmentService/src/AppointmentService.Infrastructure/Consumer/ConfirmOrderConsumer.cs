using MassTransit;
using OrderService.Contracts.Events;

namespace OrderService.Infrastructure.Consumer
{
    public sealed class ConfirmOrderConsumer : IConsumer<PaymentProcessedEvent>
    {
        public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            //throw new NotImplementedException();

            // confirmar la orden asociada al pago procesado

            // Enviar evento de notificacion por correo


            return Task.CompletedTask;
        }
    }
}
