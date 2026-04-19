
using ProductService.Application.Abstractions.Queries;
using ProductService.Application.Products.Queries.SearchProducts;

namespace ProductService.Application.Abstractions.Persistence
{
    public interface IProductSearchReadRepository
    {
        Task<PagedResult<ProductSearchItemDto>> SearchAsync(SearchProductsQuery productSearch, CancellationToken cancellationToken);
    }
}
