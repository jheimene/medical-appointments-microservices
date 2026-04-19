namespace ProductService.Domain.Brands.ValueObjects
{
    public sealed record BrandName
    {
        public string Value { get; }
        private BrandName(string value) => Value = value;
        
        public static BrandName Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El nombre de la marca es requerido.");
            var v = input.Trim();
            Guard.Against(v.Length < 2, "", "El nombre de la marca es demasiado corto.");
            Guard.Against(v.Length > 120, "", "El nombre de la marca excede 120 caracteres.");
            return new BrandName(v);
        }

        public override string ToString() => Value;
    }
}
