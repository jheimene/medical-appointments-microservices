namespace ProductService.Domain.Products.Events
{
    public sealed record ProductCreatedDomainEvent(
        Guid ProductId,
        string Name,
        string Slug,
        string Sku,
        string Status,
        string? Description,
        string Currency,
        decimal Price,
        Guid ProductTypeId,
        Guid BrandId,
        string? Model,
        string? Tags,
        IEnumerable<Guid>? CategoryIds,
        IReadOnlyDictionary<string, string>? Attributes
        ) : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.Now;
    }
}
