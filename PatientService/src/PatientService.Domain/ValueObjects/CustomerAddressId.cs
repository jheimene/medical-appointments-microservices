namespace CustomerService.Domain.ValueObjects
{
    public readonly record struct CustomerAddressId(Guid Value)
    {
        public static CustomerAddressId NewId() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
