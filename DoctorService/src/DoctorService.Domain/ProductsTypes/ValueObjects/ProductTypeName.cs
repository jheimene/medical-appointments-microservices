
namespace ProductService.Domain.ProductsTypes.ValueObjects
{
    public sealed record ProductTypeName
    {
        public string Value { get; }

        private ProductTypeName(string value) => Value = value;

        public static ProductTypeName Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El nombre del tipo de producto es requerido.");
            var v = input.Trim();

            Guard.Against(v.Length < 2, "", "El nombre del tipo de producto es demasiado corto.");
            Guard.Against(v.Length > 120, "", "El nombre del tipo de producto excede 120 caracteres.");

            return new ProductTypeName(v);
        }

        public override string ToString() => Value;
    }
}
