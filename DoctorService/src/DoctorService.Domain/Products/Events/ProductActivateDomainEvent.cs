namespace ProductService.Domain.Products.Events
{
    public sealed record ProductActivateDomainEvent(
        Guid ProductId,
        string Name,
        string Slug
        ) : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.Now;
    }
}
