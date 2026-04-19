using MassTransit;
using OrderService.Application.Dtos;
using OrderService.Application.Interfaces;
using OrderService.Contracts.Events;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Consumer
{
    public sealed class ReserveTicketConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ITicketReservationService _ticketReservationService;

        public ReserveTicketConsumer(ITicketReservationService ticketReservationService)
        {
            this._ticketReservationService = ticketReservationService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            try
            {
                var msg = context.Message;

                var zones = msg.Zonas.Select(t => new ZoneRequiredDto(t.ZoneId, t.Quantity, t.UnitPrice));

                // 1. Valida disponibilidad y reservar
                var isReserved = await _ticketReservationService.ReserveTicketsAsync(
                    msg.OrderId, msg.EventId, zones);

                if (!isReserved)
                {
                    //throw new Exception("");
                    // (Opcional) Publicar evento de fallo
                    //await context.Publish(new ());

                    return;
                }
                Console.WriteLine($"Mensaje consumido {msg.EventId}");

                // 2. Publicar evento de exito
                await context.Publish(new TicketReservedEvent(msg.OrderId, msg.EventId, Guid.NewGuid()));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al procesar mensaje: {ex.Message}");
                //_logger.LogError(ex, "error inesperado al procesar evento");
                throw;
            }
        }
    }
}
