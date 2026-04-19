namespace BffService.DTOs
{
    public sealed record ErrorResponse(
        string Code,
        string Message
    );
}
