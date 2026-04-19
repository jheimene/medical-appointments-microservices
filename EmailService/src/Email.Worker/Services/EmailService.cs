using Email.Worker.Abstractions;
using Email.Worker.Email;
using Email.Worker.Models;
using Microsoft.Extensions.Options;

namespace Email.Worker.Services
{

    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailService> _logger;
        private string Subject { get; init; }
        public EmailService(IEmailSender emailSender, ILogger<EmailService> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
            Subject = EmailServiceSettings.FromEmail;
        }

        public async Task ProcessEmailAsync(UserCreatedEvent request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Enviando email a {To} con asunto {Subject}", request.Email, Subject);


            var body = $"""
                <h1>Hola {request.FullName} </h1>
                <p>Tu cuenta fue creado exitosamente.</p> <br>
                <b>Usuario: {request.UserName} <br>
                Por valor haz clic sobre este enlace para confirmar tu correo electronico<br>
                Muchas gracias, <b>Galaxy</b>
                """;

            await _emailSender.SendEmailAsync(
                request.Email,
                Subject,
                body,
                true,
                cancellationToken);

            _logger.LogInformation("Email enviado correctamente a {To}", request.Email);
        }

    }
}
