namespace ProductService.Application.Abstractions.Queries
{
    public sealed record ProductSearchCriteria(
       string? Query,
       Guid? BrandId,
       Guid? CategoryId,
       string? Model,
       string? Size,
       string? Color,
       decimal? MinPrice,
       decimal? MaxPrice,
       string? Currency,
       IReadOnlyDictionary<string, string>? Attributes,
       int Page = 1,
       int PageSize = 20,
       ProductSort Sort = ProductSort.Relevance,
       bool Desc = false);
}
