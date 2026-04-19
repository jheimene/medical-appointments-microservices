using ErrorOr;
using MediatR;
using ProductService.Application.Abstractions.Queries;

namespace ProductService.Application.Products.Queries.SearchProducts
{
    public sealed record SearchProductsQuery(
        string? Text,
        string? ProductTypeId,
        string? BrandId,
        string? Model,
        int Take = 50,
        DateTime? LastStart = null,
        Guid? LastId = null
    //int Page = 1,
    //int PageSize = 20,
    )
     : IRequest<ErrorOr<PagedResult<ProductSearchItemDto>>>;

    //public sealed record SearchProductsQuery(ProductSearchCriteria Criteria)
    // : IRequest<ErrorOr<PagedResult<ProductSearchItem>>>;
}
