namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record Tag
    {
        public string Value { get; }

        private Tag(string value) => Value = value;

        public static Tag Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "product.tags.required", "El tag es requerido.");
            var v = input.Trim().ToLowerInvariant();

            Guard.Against(v.Length > 40, "product.tags.maxlength", "El tag excede 40 caracteres.");
            return new Tag(v);
        }

        public override string ToString() => Value;
    }
}
