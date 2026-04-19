namespace OrderService.Contracts.Dtos
{
    public sealed record PaymentRequest(
        string method,
        string currency,
        decimal amount
    );
}
