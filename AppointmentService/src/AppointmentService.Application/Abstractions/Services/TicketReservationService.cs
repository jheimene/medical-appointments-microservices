using MassTransit;
using OrderService.Application.Dtos;
using OrderService.Application.Interfaces;
using OrderService.Contracts.Events;

namespace OrderService.Application.Services
{
    public sealed class TicketReservationService : ITicketReservationService
    {
        private readonly ITicketReservationRepository _ticketReservationRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public TicketReservationService(
            ITicketReservationRepository ticketReservationRepository,
            IPublishEndpoint publishEndpoint
        )
        {
            this._ticketReservationRepository = ticketReservationRepository;
            this._publishEndpoint = publishEndpoint;
        }

        public async Task<bool> ReserveTicketsAsync(Guid orderId, Guid eventId, IEnumerable<ZoneRequiredDto> zones, CancellationToken cancellationToken = default)
        {
            var isAvailable = await _ticketReservationRepository.CheckAvailabilityAsync(eventId, zones);

            if (!isAvailable)
                return false;

            await _ticketReservationRepository.ReservedAsync(eventId, zones);

            var ticketReservedEvent = new TicketReservedEvent(orderId, eventId, Guid.NewGuid());

            // Envio del evento de tickets reservados: Saga Coreografiado
            //await _publishEndpoint.Publish(ticketReservedEvent, cancellationToken);

            return true;
        }
    }
}
