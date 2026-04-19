using BffService.DTOs;
using BffService.Interfaces;

namespace BffService.Clients
{
    public sealed class PaymentServiceClient : IPaymentServiceClient
    {
        private readonly HttpClient _httpClient;

        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId, CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<PaymentDto>(
                $"/api/payments/{paymentId}",
                cancellationToken);
        }
    }
}
