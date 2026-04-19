namespace CustomerService.Domain.ValueObjects
{
    public readonly record struct CustomerId(Guid Value)
    {
        public static CustomerId NewId() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
