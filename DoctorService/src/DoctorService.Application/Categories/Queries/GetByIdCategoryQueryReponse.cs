namespace ProductService.Application.Categories.Queries
{
    public sealed record GetByIdCategoryQueryReponse(
        Guid CategoryId,
        string Name,
        string Slug,
        Guid? ParentId,
        bool IsActive
        )
    {
    }
}
