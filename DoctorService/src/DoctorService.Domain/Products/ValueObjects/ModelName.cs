namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record ModelName
    {

        public string Value { get; }

        private ModelName(string value) => Value = value;

        public static ModelName Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "product.model.required", "El modelo es requerido.");
            var v = input.Trim();

            Guard.Against(v.Length > 100, "product.model.maxlength", "El modelo excede 100 caracteres.");
            return new ModelName(v);
        }

        public override string ToString() => Value;
    }
}
