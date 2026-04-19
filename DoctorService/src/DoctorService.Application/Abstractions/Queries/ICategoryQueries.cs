namespace ProductService.Application.Abstractions.Queries
{
    public interface ICategoryQueries
    {
        Task<IReadOnlyList<CategoryTreeItem>> GetTreeAsync(CancellationToken ct = default);
    }
}
