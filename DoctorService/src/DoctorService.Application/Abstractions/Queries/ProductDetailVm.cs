namespace ProductService.Application.Abstractions.Queries
{
    public sealed record ProductDetailVm(
        Guid ProductId,
        string Name,
        string Slug,
        string? Description,
        string BrandName,
        string? Model,
        IReadOnlyList<string> Categories,
        IReadOnlyList<KeyValuePair<string, string>> Attributes,
        IReadOnlyList<string> Images
        );
        //IReadOnlyList<ProductVariantVm> Variants);
}
