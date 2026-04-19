namespace Email.Api.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
        string to,
        string subject,
        string htmlBody,
        string? plainTextBody = null,
        CancellationToken cancellationToken = default);
    }
}
