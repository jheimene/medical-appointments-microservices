namespace Email.Api.Models
{
    public class SmtpOptions
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string FromName { get; set; } = default!;
        public string FromEmail { get; set; } = default!;
    }
}
