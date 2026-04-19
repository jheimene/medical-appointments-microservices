namespace ProductService.Domain.ProductsTypes.ValueObjects
{
    public readonly record struct ProductTypeId(Guid Value)
    {
        public static ProductTypeId NewId() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
