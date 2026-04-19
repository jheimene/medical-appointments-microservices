namespace ProductService.Domain.Brands.ValueObjects
{
    public readonly record struct BrandId(Guid Value)
    {
        public static BrandId NewId() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
