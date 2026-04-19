namespace ProductService.Domain.Products.ValueObjects
{
    public sealed record Money
    {
        public decimal Amount { get; }
        public Currency Currency { get; }

        private Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, Currency currency)
        {
            if (amount < 0)
                throw new InvalidValueObjectException("", "El monto no puede ser negativo.", new Dictionary<string, string[]> { });

            // Redondeo uniforme
            var rounded = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
            return new Money(rounded, currency);
        }

        public override string ToString() => $"{Amount} {Currency.Code}";
    }
}
