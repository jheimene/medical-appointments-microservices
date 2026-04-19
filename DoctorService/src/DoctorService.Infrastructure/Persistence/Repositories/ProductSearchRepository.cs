using ProductService.Application.Abstractions.Persistence;
using ProductService.Infrastructure.Persistence.Contexts;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    public class ProductSearchRepository : IProductSearchRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ProductSearchRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task AddAsync(ProductSearch productSearch, CancellationToken cancellationToken)
        {
            _applicationDbContext.ProductSearches.Add(productSearch);
            return Task.CompletedTask;
        }
    }
}
