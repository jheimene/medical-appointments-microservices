namespace OrderService.Application.Dtos
{
    public sealed record PaymentRequestDto (
        string Method,
        string Currency,
        decimal Amount
    );       
}
