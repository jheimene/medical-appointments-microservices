namespace ProductService.Domain.ProductsTypes.ValueObjects
{
    public sealed record ProductTypeCode
    {
        public string Value { get; }
        private ProductTypeCode(string value) => Value = value;

        public static ProductTypeCode Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El código del tipo de producto es requerido.");
            var v = input.Trim();
            Guard.Against(v.Length < 2, "", "El código del tipo de producto es demasiado corto.");
            Guard.Against(v.Length > 20, "", "El código del tipo de producto excede los 20 caracteres.");
            return new ProductTypeCode(v);
        }

        public override string ToString() => Value;
    }
}
