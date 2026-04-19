namespace ProductService.Domain.Categories.ValueObjects
{
    public readonly record struct CategoryId(Guid Value)
    //public sealed record CategoryId(Guid Value)
    {
        public static CategoryId NewId() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
