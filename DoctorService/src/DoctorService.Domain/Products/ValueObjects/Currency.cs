namespace ProductService.Domain.Products.ValueObjects
{
    public readonly record struct Currency(string Code)
    {
        public static Currency Create(string code)
        {
            Guard.AgainstNullOrWhiteSpace(code, "", "La moneda es requerida.");
            var v = code.Trim().ToUpperInvariant();

            Guard.Against(v.Length != 3, "", "Currency debe ser ISO de 3 letras (ej: PEN, USD).");
            return new Currency(v);
        }

        public override string ToString() => Code;
    }
}
