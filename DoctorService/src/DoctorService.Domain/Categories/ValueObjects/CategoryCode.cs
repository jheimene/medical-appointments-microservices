namespace ProductService.Domain.Cat.ValueObjects
{
    public sealed record CategoryCode
    {
        public string Value { get; }
        private CategoryCode(string value) => Value = value;

        public static CategoryCode Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "El código de la categoría es requerido.");
            var v = input.Trim();
            Guard.Against(v.Length < 2, "", "El código de la categoría es demasiado corto.");
            Guard.Against(v.Length > 20, "", "El código de la categoría excede los 20 caracteres.");
            return new CategoryCode(v);
        }

        public override string ToString() => Value;
    }
}
