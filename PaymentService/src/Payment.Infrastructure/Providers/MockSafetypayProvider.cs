

using Microsoft.Extensions.Logging;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.Utils;

namespace PaymentService.Infrastructure.Providers
{
    public class MockSafetypayProvider : IPaymentProvider
    {
        private readonly ILogger<MockSafetypayProvider> _logger;

        public MockSafetypayProvider(ILogger<MockSafetypayProvider> logger)
        {
            _logger = logger;
        }

        public async Task<string> ProccessPaymentAsync(decimal amout, string currency, string user)
        {
            _logger.LogInformation("Procesando pago mock con Safetypay para el usuario {User}", user);

            var random = new Random();
            int numero = random.Next(0, 11);
            if (numero >= 0 && numero <= 1)
            {
                throw new Exception("Ocurrio un error al procesar el pago con Paypal"); ;
            }

            await Task.Delay(500); // Simula llamada a API

            var paymentId = IdGenerator.GenerateRandomId();

            _logger.LogInformation("Pago simulado exitoso. PaymentId: {PaymentId}", paymentId);

            return paymentId;
        }
    }
}
