using OrderService.Application.Abstractions.Interfaces;
using OrderService.Infrastructure.Persistence.Contexts;
using OrderService.Infrastructure.Persistence.Mongo;
using OrderService.Infrastructure.Persistence.Repositories;

namespace OrderService.Infrastructure.Persistence.Factories
{
    public class OrderRepositoryProvider : IOrderRepositoryProvider
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly MongoDbContext _mongoDbContext;

        //private readonly IDictionary<string, IOrderRepository> _repositories;

        public OrderRepositoryProvider(OrderDbContext orderDbContext, MongoDbContext mongoDbContext)
        {
            _orderDbContext = orderDbContext;
            _mongoDbContext = mongoDbContext;
        }

        public IOrderRepository GetOrderRepository(RepositoryType type, CancellationToken cancellationToken = default)
        {
            return type switch
            {
                RepositoryType.Mongo => new OrderRepositoryMongo(_mongoDbContext),
                RepositoryType.MongoEf => new OrderRepositoryMongoEF(_orderDbContext),
                //null or "" => new OrderRepositoryMongoEF(_orderDbContext),
                _ => throw new NotImplementedException($"The repository type '{type}' is not implemented.")
            };
        }
    }

}
