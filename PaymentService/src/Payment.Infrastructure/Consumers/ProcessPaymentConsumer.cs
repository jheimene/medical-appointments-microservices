using MassTransit;
using MediatR;
using Orders.Contracts.Events;
using Payments.Application.Handlers.Payments.Commands.CreatePayment;

namespace Payments.Infrastructure.Consumers
{
    public sealed class ProcessPaymentConsumer : IConsumer<TicketReservedEvent>
    {
        private readonly IMediator _mediator;

        public ProcessPaymentConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<TicketReservedEvent> context)
        {
            var msg = context.Message;

            var command = new CreatePaymentCommand(
                msg.Payment.method,
                msg.Payment.currency,
                msg.Payment.amount,
                msg.EventId,
                msg.UserId.ToString()
            );

            var (paymentId, success) = await _mediator.Send(command);

            if (success)
            {
                await context.Publish(new PaymentProcessedEvent(msg.OrderId, msg.EventId));
            } else
            {
                await context.Publish(new PaymentFailedEvent(msg.OrderId, msg.EventId));
            }

        }
    }
}
