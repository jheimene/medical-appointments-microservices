namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record ImageUrl
    {
        public string Value { get; }

        private ImageUrl(string value) => Value = value;

        public static ImageUrl Create(string input)
        {
            Guard.AgainstNullOrWhiteSpace(input, "", "La URL de imagen es requerida.");
            var v = input.Trim();

            //if (!Uri.TryCreate(v, UriKind.Absolute, out var uri) ||
            //    (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
            //    throw new InvalidValueObjectException("", "URL de imagen inválida (debe ser http/https).", new Dictionary<string, string[]>());

            return new ImageUrl(v);
        }

        public override string ToString() => Value;
    }
}
