namespace ProductService.Application.Abstractions.Persistence
{
    public interface IRepository<TAggregate, TId>
    {
        Task<TAggregate?> GetByIdAsync(TId id, CancellationToken ct = default);
        Task AddAsync(TAggregate entity, CancellationToken ct = default);
        void Update(TAggregate entity);
        void Remove(TAggregate entity);
    }
}
