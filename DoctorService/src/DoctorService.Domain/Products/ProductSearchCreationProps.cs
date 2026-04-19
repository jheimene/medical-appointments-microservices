
namespace ProductService.Domain.Products
{
    public sealed record ProductSearchCreationProps(
        Guid ProductId,
        string Name,
        string Slug,
        string Sku,
        string Currency,
        decimal Price,
        string Status,
        string? Description,
        Guid ProductTypeId,
        string ProductType,
        Guid BrandId,
        string Brand,
        string? Model,
        string? Tags,
        string? Categories,
        string? Attributes
    );
}
