namespace BffService.DTOs
{
    public sealed record CustomerSummary(
        Guid CustomerId,
        string FullName,
        string Email
    );
}
