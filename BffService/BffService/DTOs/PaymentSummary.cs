namespace BffService.DTOs
{
    public sealed record PaymentSummary(
        Guid PaymentId,
        string Status,
        decimal Amount,
        string Currency
    );
}
