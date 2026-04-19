
namespace Email.Worker.Email
{
    public sealed class EmailServiceSettings
    {
        public const string SectionName = "EmailService";
        public const string FromEmail = "ronald.tc14@gmail.com";
        public string BaseUrl { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
        public EmailServiceSettings() { }
    }
}
