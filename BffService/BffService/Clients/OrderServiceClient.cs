using BffService.DTOs;
using BffService.Interfaces;

namespace BffService.Clients
{
    public sealed class OrderServiceClient : IOrderServiceClient
    {
        private readonly HttpClient _httpClient;

        public OrderServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<OrderDto>(
                $"/api/orders/{orderId}",
                cancellationToken);
        }
    }
}
