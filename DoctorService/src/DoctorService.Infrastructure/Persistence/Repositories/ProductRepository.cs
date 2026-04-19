using ProductService.Infrastructure.Persistence.Contexts;
using ProductService.Application.Abstractions.Persistence;
using ProductService.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            _dbContext.Add(product);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByIdAsync(ProductId productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.AnyAsync(p => p.Id == productId);
        }

        public async Task<bool> ExistsBySkuAsync(Sku sku, ProductId? excludeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.AnyAsync(p => p.Sku == sku && (!excludeId.HasValue || p.Id != excludeId.Value), cancellationToken);
        }

        public async Task<bool> ExistsBySlugAsync(Slug slug, ProductId? excludeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.AnyAsync(p => p.Slug == slug && (!excludeId.HasValue || p.Id != excludeId.Value), cancellationToken);  
        }

        public async Task<Product?> GetByIdAsync(ProductId productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.FindAsync(new object[] { productId }, cancellationToken);
        }

        public async Task<Product?> GetByIdForUpdateAsync(ProductId productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Include(p => p.CategoryIds)
                .Include(p => p.Tags)
                .Include(p => p.Attributes)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);
        }

        public Task<Product?> GetBySkuAsync(Sku sku, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(Product product)
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
