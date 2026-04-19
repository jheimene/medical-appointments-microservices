
namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record ProductName
    {
        public string Value { get; }
        private ProductName(string value) => Value = value;

        public static ProductName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidValueObjectException("product.name.required", "El nombre del producto es requerido", new Dictionary<string, string[]>());

            if (value.Length > 100)
                throw new InvalidValueObjectException("product.name.maxlength", "El nombre del producto no puede exceder los 100 caracteres", new Dictionary<string, string[]>());

            return new ProductName(value.Trim());
        }

        public override string ToString() => Value;
    }
}
