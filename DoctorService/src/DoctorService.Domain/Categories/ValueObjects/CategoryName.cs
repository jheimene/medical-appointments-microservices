
namespace ProductService.Domain.Categories.ValueObjects
{
    public sealed record CategoryName
    {
        public string Value { get; }

        private CategoryName(string value) => Value = value;

        public static CategoryName Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El nombre de la categoría es requerido.");
            var v = input.Trim();

            Guard.Against(v.Length < 2, "", "El nombre de la categoría es demasiado corto.");
            Guard.Against(v.Length > 120, "", "El nombre de la categoría excede 120 caracteres.");

            return new CategoryName(v);
        }

        public override string ToString() => Value;
    }
}
