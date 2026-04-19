using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories
{
    public interface IEventInventoryRepository
    {
        Task CreateOneAsync(EventInventory eventInventory);
        Task CreateManyAsync(IEnumerable<EventInventory> eventInventories);
        Task<IEnumerable<EventInventory>> FindByEventIdAsync(Guid eventId);
    }
}
