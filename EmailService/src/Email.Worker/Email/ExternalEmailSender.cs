
using Email.Worker.Abstractions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Email.Worker.Email
{
    public sealed class ExternalEmailSender : IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly EmailServiceSettings _settings;
        private readonly ILogger<ExternalEmailSender> _logger;

        public ExternalEmailSender(
            HttpClient httpClient,
            IOptions<EmailServiceSettings> options,
            ILogger<ExternalEmailSender> logger
            )
        {
            _httpClient = httpClient;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                to,
                subject,
                htmlBody = body,
                plainTextBody = "",
                isHtml
            };


            _logger.LogInformation("Llamando servicio externo de email para {To}", to);
            _logger.LogInformation("BaseUrl configurado para EmailService: {BaseUrl}", _settings.BaseUrl);


            try
            {
                //using var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/send-email")
                //{
                //    Content = JsonContent.Create(payload)
                //};

                //request.Headers.Add("x-api-key", _settings.ApiKey);

                //using var response = await _httpClient.SendAsync(request, cancellationToken);

                using var response = await _httpClient.PostAsJsonAsync("/send-email", payload, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError("Fallo al enviar correo. Status: {StatusCode}, Body: {Body}",
                        response.StatusCode, errorContent);
                    throw new InvalidOperationException("No se pudo enviar el correo");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            _logger.LogInformation("Servicio externo de email respondió OK para {To}", to);
        }
    }
}
