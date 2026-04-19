using System.Text.RegularExpressions;

namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record Slug
    {
        private static readonly Regex Allowed = new("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);

        public string Value { get; }

        private Slug(string value) => Value = value;

        public static Slug Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "product.slug.required", "El slug es requerido.");
            var v = input.Trim().ToLowerInvariant();

            Guard.Against(v.Length > 160, "product.slug.maxlength", "El slug excede 160 caracteres.");

            // Normalización mínima: espacios -> guiones
            v = Regex.Replace(v, "\\s+", "-");

            //if (!Allowed.IsMatch(v))
            //    throw new InvalidValueObjectException("product.slug.invalid", "Slug inválido. Usa letras minúsculas, números y guiones (sin doble guión).", new Dictionary<string, string[]>());

            return new Slug(v);
        }

        public override string ToString() => Value;

    }
}
