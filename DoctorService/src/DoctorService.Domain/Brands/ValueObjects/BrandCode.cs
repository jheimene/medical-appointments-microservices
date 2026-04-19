namespace ProductService.Domain.Brands.ValueObjects
{
    public sealed record BrandCode
    {
        public string Value { get; }
        private BrandCode(string value) => Value = value;

        public static BrandCode Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El código de la marca es requerido.");
            var v = input.Trim();
            Guard.Against(v.Length < 2, "", "El código de la marca es demasiado corto.");
            Guard.Against(v.Length > 20, "", "El código de la marca excede  los 20 caracteres.");
            return new BrandCode(v);
        }

        public override string ToString() => Value;
    }
}
