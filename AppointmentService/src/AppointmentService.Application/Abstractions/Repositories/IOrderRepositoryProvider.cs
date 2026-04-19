namespace OrderService.Application.Abstractions.Interfaces
{
    public interface IOrderRepositoryProvider
    {
        IOrderRepository GetOrderRepository(RepositoryType type, CancellationToken cancellationToken = default);
    }

    public enum RepositoryType
    {
        MongoEf,
        Mongo
    }
}
