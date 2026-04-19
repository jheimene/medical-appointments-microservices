
namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record AttributeValue
    {
        public string Value { get; }

        private AttributeValue(string value) => Value = value;

        public static AttributeValue Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El valor del atributo es requerido.");
            var v = input.Trim();

            Guard.Against(v.Length > 200, "", "El valor del atributo excede 200 caracteres.");
            return new AttributeValue(v);
        }

        public override string ToString() => Value;
    }
}
