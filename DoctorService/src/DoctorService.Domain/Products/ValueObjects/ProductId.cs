namespace ProductService.Domain.Products.ValueObjects
{
    public readonly record struct ProductId(Guid Value)
    {
        public static ProductId NewId() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
