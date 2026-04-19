using MassTransit;
using Orders.Contracts.Comamnds;
using Orders.Contracts.Events;

namespace Orders.Saga
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {

        public State OrderInitialize { get; private set; }
        public State TicketReserved { get; private set; }
        public State TicketFailed { get; private set; }
        public State PaymentProcessed { get; private set; }
        public State OrderCompleted { get; private set; }
        public State OrderFailed { get; private set; }
        //public State Compensating { get; private set; }


        public Event<InitializeOrderCommand> InitializeOrderCommand { get; private set; }
        public Event<TicketReservedEvent> TicketReservedEvent { get; private set; }       
        public Event<TicketReservedFailedEvent> TicketReservedFailedEvent { get; set; }
        public Event<PaymentProcessedEvent> PaymentProcessedEvent { get; private set; }
        public Event<PaymentFailedEvent> PaymentFailedEvent { get; set; }
        public Event<OrderCompletedEvent> OrderCompletedEvent { get; private set; }
        public Event<OrderFailedEvent> OrderFailedEvent { get; private set; }

        //public Schedule<OrderState, OrchestratorTimeout> Timeout { get; private set; }

        public OrderStateMachine()
        {
            Console.WriteLine("Configuring Order State Machine...");

            InstanceState(x => x.CurrentState);

            // Configuración de eventos y transiciones de estado aquí
            Event(() => InitializeOrderCommand, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => TicketReservedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => TicketReservedFailedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => PaymentProcessedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => PaymentFailedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => OrderCompletedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => OrderFailedEvent, x => x.CorrelateById(context => context.Message.OrderId));


            Initially(
                When(InitializeOrderCommand)
                .Then(context =>
                {
                    Console.WriteLine($"InitializeOrderCommand received in Saga with order {context.Message.OrderId}");
                    context.Saga.EventId = context.Message.EventId;
                    context.Saga.CustomerId = context.Message.CustomerId;
                    context.Saga.Payment = context.Message.Payment;
                    context.Saga.Zones = context.Message.Zones;
                    context.Saga.CreatedAt = DateTime.Now;
                    context.Saga.UserId = "system"; // Asignar un valor predeterminado o extraer del contexto si está disponible
                })
                // Enviar comando para reservar tickets
                .Send(new Uri("queue:command.reserve.ticket.queue"), context => new ReserveTicketCommand(
                    context.Saga.CorrelationId,
                    context.Saga.EventId,
                    context.Saga.Zones
                ))
                .TransitionTo(OrderInitialize)
            );

            During(OrderInitialize,
                When(TicketReservedEvent)
                    .Then(context =>
                    {
                        Console.WriteLine($"TicketReservedEvent received in Saga with order {context.Message.OrderId}");
                    })
                    // Enviar comando para procesar pago             
                    .Send(new Uri("queue:command.process.payment.queue"), context => new ProcessPaymentCommand(
                        context.Saga.CorrelationId,
                        context.Saga.EventId,
                        context.Saga.CustomerId,
                        context.Saga.Payment
                    ))
                    .TransitionTo(TicketReserved),

                When(TicketReservedFailedEvent)
                    .Then(context =>
                    {
                        Console.WriteLine($"TicketReservedFailedEvent received in Saga with order {context.Message.OrderId}");
                        // Si la reserva de tickets falla, finalizar la saga o manejar el error según sea necesario
                    }) 
                    .Send(new Uri("queue:command.cancel.order.queue"), context => new CancelOrderCommand(
                            context.Saga.CorrelationId,
                            context.Saga.EventId
                    ))
                    .TransitionTo(TicketFailed)
            );


            During(TicketReserved,
                When(PaymentProcessedEvent)
                    .Then(context =>
                    {
                        Console.WriteLine($"Pago procesado correctamente");
                    })
                    .Send(new Uri("queue:command.confirm.order.queue"), context => new ConfirmOrderCommand(
                        context.Saga.CorrelationId,
                        context.Saga.EventId
                    ))
                    .TransitionTo(OrderCompleted),

                When(PaymentFailedEvent)
                    .ThenAsync(async context =>
                    {
                        Console.WriteLine($"Error al realizar el pago");

                        // Si el pago falla, enviar comando para cancelar la orden y liberar los tickets reservados

                        // 1. Enviar comando para cancelar la orden
                        await context.Send(new Uri("queue:command.cancel.order.queue"), new CancelOrderCommand(
                            context.Saga.CorrelationId,
                            context.Saga.EventId
                        ));

                        //await context.Send<CancelOrderCommand>(new
                        //{
                        //    CorrelationId = context.Saga.CorrelationId,
                        //    OrderId = context.Saga.EventId
                        //});

                        // 2. Enviar comando para liberar los tickets reservados
                        await context.Send(new Uri("queue:command.release.ticket.queue"), new ReleaseTicketCommand(
                            context.Saga.CorrelationId,
                            context.Saga.EventId,
                            context.Saga.Zones
                        ));
                      
                    })
                    .TransitionTo(OrderFailed)
            );


            During(TicketFailed,
                  When(OrderFailedEvent)
                      .Then(ctx => Console.WriteLine("Orden cancelada"))
                      .Finalize()
            );

            During(OrderCompleted,
                  When(OrderCompletedEvent)
                      .Then(ctx => Console.WriteLine("Orden finalizada"))
                      .Finalize()
            );

            During(OrderFailed,
                When(OrderFailedEvent)
                    .Then(ctx => Console.WriteLine("Orden cancelada"))
                    .Finalize()
            );

            SetCompletedWhenFinalized();

        }
    }
}
