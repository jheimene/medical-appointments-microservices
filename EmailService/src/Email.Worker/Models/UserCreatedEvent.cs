namespace Email.Worker.Models
{
    public sealed record UserCreatedEvent(
        String UserId,
        String FullName,
        String Email,
        String UserName
    );
}
