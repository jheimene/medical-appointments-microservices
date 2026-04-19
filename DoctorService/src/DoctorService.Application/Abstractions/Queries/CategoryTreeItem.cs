namespace ProductService.Application.Abstractions.Queries
{
    public sealed record CategoryTreeItem(
        Guid CategoryId,
        string Name,
        string Slug,
        Guid? ParentId,
        int Level
    );
}
