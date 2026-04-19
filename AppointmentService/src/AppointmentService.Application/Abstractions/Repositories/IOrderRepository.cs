using OrderService.Domain.Orders;

namespace OrderService.Application.Abstractions.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> GetByCustomerIdAsync(Guid customerId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task CreateAsync(Order order, CancellationToken cancellationToken = default);
        Task<bool> ExistsByIdEmpotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);
        Task<Order?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
