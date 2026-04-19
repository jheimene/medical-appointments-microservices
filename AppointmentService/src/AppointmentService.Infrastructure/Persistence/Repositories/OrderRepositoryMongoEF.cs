using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Interfaces;
using OrderService.Domain.Orders;
using OrderService.Infrastructure.Persistence.Contexts;

namespace OrderService.Infrastructure.Persistence.Repositories
{
    public class OrderRepositoryMongoEF : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepositoryMongoEF(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default) => await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

        public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(Guid customerId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            return await _context.Orders.AsNoTracking()
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedAt)   
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
            //await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);

        public async Task<bool> ExistsByIdEmpotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.AnyAsync(x => x.IdempotencyKey == idempotencyKey, cancellationToken);
        }

        public async Task<Order?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.IdempotencyKey == idempotencyKey, cancellationToken);
        }
    }
}
