using MassTransit;
using MediatR;
using Orders.Contracts.Comamnds;
using Orders.Contracts.Events;
using PaymentService.Application.Handlers.Payments.Commands.CreatePayment;

namespace PaymentService.Infrastructure.Consumers
{
    public sealed class ProcessPaymentSagaConsumer : IConsumer<ProcessPaymentCommand>
    {
        private readonly IMediator _mediator;

        public ProcessPaymentSagaConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ProcessPaymentCommand> context)
        {
            var msg = context.Message;

            var command = new CreatePaymentCommand(
                msg.Payment.method,
                msg.Payment.currency,
                msg.Payment.amount,
                msg.EventId,
                msg.CustomerId.ToString()
            );

            var (paymentId, success) = await _mediator.Send(command);

            if (success)
            {
                await context.Publish(new PaymentProcessedEvent(msg.OrderId, msg.EventId));
            }
            else
            {
                await context.Publish(new PaymentFailedEvent(msg.OrderId, msg.EventId));
            }

        }
    }
}
