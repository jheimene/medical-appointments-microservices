
using ProductService.Application.Abstractions.Persistence;
using ProductService.Domain.Brands;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Infrastructure.Persistence.Contexts;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    public sealed class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
       
        public BrandRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task AddAsync(Brand brand, CancellationToken cancellationToken = default)
        {
            _dbContext.Add(brand);
            return Task.CompletedTask;
        }

        public async Task<Brand?> GetByIdAsync(BrandId brandId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Brands.FindAsync([brandId], cancellationToken).AsTask();
        }

        public Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Brand brand)
        {
            throw new NotImplementedException();
        }
    }
}
