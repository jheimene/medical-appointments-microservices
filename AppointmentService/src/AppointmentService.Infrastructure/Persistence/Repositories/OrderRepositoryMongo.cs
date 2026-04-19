using MongoDB.Driver;
using OrderService.Application.Abstractions.Interfaces;
using OrderService.Domain.Orders;
using OrderService.Infrastructure.Persistence.Mongo;


namespace OrderService.Infrastructure.Persistence.Repositories
{
    public class OrderRepositoryMongo : IOrderRepository
    {
        private readonly IMongoCollection<OrderDocument> _collectionOrder;

        public OrderRepositoryMongo(MongoDbContext context)
        {
            _collectionOrder = context.Orders;
        }

        public async Task CreateAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _collectionOrder.InsertOneAsync(order.ToDocument());
        }

        public async Task<bool> ExistsByIdEmpotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
        {
            return await _collectionOrder.Find(o => o.IdempotencyKey == idempotencyKey).AnyAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(Guid customerId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 20 : pageSize;

            var filtro = Builders<OrderDocument>.Filter.Eq(e => e.CustomerId, customerId);

            var documents = await _collectionOrder
                .Find(filtro)
                .SortByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return [.. documents.Select(o => o.ToDomain())];
        }

        public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var document = await _collectionOrder.Find(o => o.OrderId == orderId).FirstOrDefaultAsync(cancellationToken);
            return document?.ToDomain();
        }

        public async Task<Order?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
        {
            var document = await _collectionOrder.Find(o => o.IdempotencyKey == idempotencyKey).FirstOrDefaultAsync(cancellationToken);
            return document?.ToDomain();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }
}
