using ProductService.Application.Abstractions.Persistence;
using ProductService.Domain.ProductsTypes;
using ProductService.Domain.ProductsTypes.ValueObjects;
using ProductService.Infrastructure.Persistence.Contexts;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    public class ProductTypeRepository(ApplicationDbContext dbContext) : IProductTypeRepository
    {
        public Task AddAsync(ProductType productType, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductType?> GetByIdAsync(ProductTypeId productTypeId, CancellationToken cancellation = default)
        {
            return await dbContext.ProductsTypes.FindAsync(productTypeId, cancellation);
        }

        public Task<ProductType?> GetByNameAsync(string name, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductType productType)
        {
            throw new NotImplementedException();
        }
    }
}
