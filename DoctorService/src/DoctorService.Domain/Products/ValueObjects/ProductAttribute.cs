
namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record ProductAttribute
    {
        public AttributeKey Key { get; }
        public AttributeValue Value { get; }
        public bool IsFilterable { get; } // útil para el buscador

        private ProductAttribute(AttributeKey key, AttributeValue value, bool isFilterable)
        {
            Key = key;
            Value = value;
            IsFilterable = isFilterable;
        }

        public static ProductAttribute Create(string key, string value, bool isFilterable = true)
            => new(AttributeKey.Create(key), AttributeValue.Create(value), isFilterable);

        //protected override IEnumerable<object?> GetEqualityComponents()
        //{
        //    yield return Key;
        //    yield return Value;
        //    yield return IsFilterable;
        //}

        public override string ToString() => $"{Key}={Value}";
    }
}
