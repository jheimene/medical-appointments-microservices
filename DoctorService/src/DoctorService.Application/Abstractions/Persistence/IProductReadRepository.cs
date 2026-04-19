
using ProductService.Application.Abstractions.Queries;
using ProductService.Application.Products.Queries.SearchProducts;

namespace ProductService.Application.Abstractions.Persistence
{
    public interface IProductReadRepository
    {
        //Task<PagedResult<ProductSearchItem>> SearchAsync(ProductSearchCriteria criteria, CancellationToken ct = default);
        Task<PagedResult<ProductSearchItemDto>> SearchAsync(SearchProductsQuery query, CancellationToken cancellationToken = default);
    }
}
