using ErrorOr;
using MediatR;
using ProductService.Application.Abstractions.Queries;
namespace ProductService.Application.Products.Queries.SearchProducts
{
    public sealed class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, ErrorOr<PagedResult<ProductSearchItemDto>>>
    {
        //private readonly IProductQueries _queries;        
        //public SearchProductsQueryHandler(IProductQueries queries) => _queries = queries;
        private readonly IProductSearchReadRepository _productSearchReadRepository;

        public SearchProductsQueryHandler(IProductSearchReadRepository productReadRepository) => _productSearchReadRepository = productReadRepository;

        public async Task<ErrorOr<PagedResult<ProductSearchItemDto>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                //return ErrorOr<PagedResult<ProductSearchItem>>.From(Error.Validation(
                //    code: "SearchProductsQuery.Null",
                //    description: "The search products query cannot be null."
                //));
                throw new ArgumentNullException(nameof(request), "The search products query cannot be null.");
            }

            return await _productSearchReadRepository.SearchAsync(request, cancellationToken);

        }

    }
}
