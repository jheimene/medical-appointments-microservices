using ProductService.Domain.Categories.ValueObjects;
using System.Text.RegularExpressions;

namespace ProductService.Domain.Brands.ValueObjects
{
    public sealed record BrandSlug
    {
        private static readonly Regex Allowed = new("^[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$", RegexOptions.Compiled);

        public string Value { get; }

        private BrandSlug(string value) => Value = value;

        public static BrandSlug Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El slug de la brand es requerido.");
            var v = input.Trim().ToLowerInvariant();

            Guard.Against(v.Length > 160, "", "El slug excede 160 caracteres.");

            v = Regex.Replace(v, "\\s+", "-");

            //if (!Allowed.IsMatch(v))
            //    throw new InvalidValueObjectException("", "Slug inválido. Usa minúsculas, números y guiones (sin doble guión).", new Dictionary<string, string[]>());

            return new BrandSlug(v);
        }

        public override string ToString() => Value;
    }
}
