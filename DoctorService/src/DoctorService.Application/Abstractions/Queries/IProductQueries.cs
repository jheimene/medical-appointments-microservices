namespace ProductService.Application.Abstractions.Queries
{
    public interface IProductQueries
    {
        Task<PagedResult<ProductSearchItemDto>> SearchAsync(ProductSearchCriteria criteria, CancellationToken ct = default);

        Task<ProductDetailVm?> GetDetailAsync(Guid productId, CancellationToken ct = default);

        // Autocomplete / sugerencias
        Task<IReadOnlyList<string>> SuggestAsync(string text, int take = 10, CancellationToken ct = default);
    }
}
