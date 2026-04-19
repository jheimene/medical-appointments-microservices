namespace BffService.DTOs
{
    public sealed record PaymentDto(
        Guid Id,
        string Status,
        decimal Amount,
        string Currency
    );
}
