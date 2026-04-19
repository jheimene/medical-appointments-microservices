namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record AttributeKey
    {
        public string Value { get; }

        private AttributeKey(string value) => Value = value;

        public static AttributeKey Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "La clave del atributo es requerida.");
            var v = input.Trim().ToLowerInvariant();

            Guard.Against(v.Length > 60, "", "La clave del atributo excede 60 caracteres.");
            return new AttributeKey(v);
        }

        public override string ToString() => Value;
    }
}
