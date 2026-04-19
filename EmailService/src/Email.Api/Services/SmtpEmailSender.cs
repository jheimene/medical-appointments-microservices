
using Email.Api.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Email.Api.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _options;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(
            IOptions<SmtpOptions> options,
            ILogger<SmtpEmailSender> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody, string? plainTextBody = null, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            // Cuerpo (texto plano + HTML)
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = plainTextBody ?? StripHtml(htmlBody)
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                // Conectar
                var secureSocket = _options.UseSsl
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.Auto;

                await client.ConnectAsync(_options.Host, _options.Port, secureSocket, cancellationToken);

                // Autenticarse si aplica
                if (!string.IsNullOrWhiteSpace(_options.User))
                {
                    await client.AuthenticateAsync(_options.User, _options.Password, cancellationToken);
                }

                // Enviar
                await client.SendAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo a {To}", to);
                throw; // aquí podrías lanzar una excepción custom o manejar reintentos
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true, cancellationToken);
                }
            }
        }

        // Utilidad sencilla para obtener texto plano
        private static string StripHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }
    }
}
