
namespace Email.Worker.Models
{
    public sealed record SendEmailRequest(
        string To,
        string Subject,
        string Body,
        bool IsHtml = true
    );
}
