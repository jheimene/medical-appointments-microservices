
using System.Text.RegularExpressions;

namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record Sku
    {
        private static readonly Regex Pattern = new("^[A-Z0-9\\-]{4,32}$", RegexOptions.Compiled);

        public string Value { get; }

        private Sku(string value) => Value = value;

        public static Sku Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El SKU es requerido.");
            var v = input.Trim().ToUpperInvariant();

            //if (!Pattern.IsMatch(v))
            //    throw new InvalidValueObjectException("", "SKU inválido. Usa A-Z, 0-9 y '-' (4 a 32 chars).", new Dictionary<string, string[]>());

            return new Sku(v);
        }

        public override string ToString() => Value;
    }
}
