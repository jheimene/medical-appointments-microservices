namespace BffService.DTOs
{
    public sealed record CustomerDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email
    );
}
