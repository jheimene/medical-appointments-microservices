
using OrderService.Application.Abstractions.Services;
using OrderService.Application.Dtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace OrderService.Infrastructure.Services
{
    public sealed class PaymentProcessService : IPaymentProcessService
    {
        private readonly HttpClient _httpClient;
        public PaymentProcessService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PaymentResult?> ProcessPaymentAsync(ProcessPaymentRequest processPayment, CancellationToken cancellation)
        {
            string json = JsonSerializer.Serialize(processPayment);

            var content = new StringContent(json, encoding: Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"/api/payments", content, cancellation);
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<PaymentResult?>(cancellation);
            }
            catch (Exception)
            {
                throw new Exception("Error al procesar el pago");
            }

        }
    }
}
