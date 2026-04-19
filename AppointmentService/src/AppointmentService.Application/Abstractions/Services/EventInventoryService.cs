
using OrderService.Application.Dtos;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace OrderService.Application.Services
{
    public class EventInventoryService : IEventInventoryService
    {
        private readonly IEventInventoryRepository _eventInventoryRepository;

        public EventInventoryService(IEventInventoryRepository eventInventoryRepository)
        {
            this._eventInventoryRepository = eventInventoryRepository;
        }

        public async Task CreateAsync(EventPublishedEvent eventPublished)
        {
            var entity = await _eventInventoryRepository.FindByEventIdAsync(eventPublished.EventId);

            if (entity.Any())
            {
                throw new Exception("Este evento ya has sido registrado");
            }

            var eventInventories = eventPublished.Zones.Select(z =>
            {
                return EventInventory.Create(
                    eventPublished.EventId,
                    eventPublished.EventName,
                    z.ZoneId,
                    z.ZoneName,
                    z.Capacity
                );
            });

            await _eventInventoryRepository.CreateManyAsync(eventInventories);
        }
    }
}
