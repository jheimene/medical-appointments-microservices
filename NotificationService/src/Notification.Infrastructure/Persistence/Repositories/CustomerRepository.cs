
using DispatchService.Domain.Interfaces;
using DispatchService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DispatchService.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateAsync(Customer customer)
        {
            _dbContext.Customers.Add(customer);
        }

        public Task DeleteAsync(CustomerId id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(documentNumber)) return false;

            documentNumber = documentNumber.Trim();

            return await _dbContext.Customers.AsNoTracking()
                .Where(e => e.Document.Number == documentNumber)
                .AnyAsync();
        }

        public Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByDocumentNumber(string documentNumber, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Customers
                .Include(a => a.Address)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
