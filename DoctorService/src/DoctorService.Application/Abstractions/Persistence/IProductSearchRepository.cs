using ProductService.Domain.Products;

namespace ProductService.Application.Abstractions.Persistence
{
    public interface IProductSearchRepository
    {
        Task AddAsync(ProductSearch productSearch, CancellationToken cancellationToken);
    }
}
