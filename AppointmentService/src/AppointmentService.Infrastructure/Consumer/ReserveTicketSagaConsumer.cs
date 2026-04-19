using MassTransit;
using OrderService.Application.Dtos;
using OrderService.Application.Interfaces;
using OrderService.Contracts.Comamnds;
using OrderService.Contracts.Events;

namespace OrderService.Infrastructure.Consumer
{
    public class ReserveTicketSagaConsumer : IConsumer<ReserveTicketCommand>
    {
        private readonly ITicketReservationService _ticketReservationService;

        public ReserveTicketSagaConsumer(ITicketReservationService ticketReservationService)
        {
            this._ticketReservationService = ticketReservationService;
        }

        public async Task Consume(ConsumeContext<ReserveTicketCommand> context)
        {
            try
            {
                var msg = context.Message;

                var zones = msg.Zones.Select(t => new ZoneRequiredDto(t.ZoneId, t.Quantity, t.UnitPrice));

                // 1. Valida disponibilidad y reservar
                var isReserved = await _ticketReservationService.ReserveTicketsAsync(
                    msg.OrderId, msg.EventId, zones);

                if (!isReserved)
                {
                    //throw new Exception("");

                    // (Opcional) Publicar evento de fallo
                    await context.Publish(new TicketReservedFailedEvent(msg.OrderId, msg.EventId));

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
