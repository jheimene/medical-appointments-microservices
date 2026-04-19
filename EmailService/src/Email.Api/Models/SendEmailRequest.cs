namespace Email.Api.Models
{
    public class SendEmailRequest
    {
        public string To { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string HtmlBody { get; set; } = default!;
        public string? PlainTextBody { get; set; }
    }
}
