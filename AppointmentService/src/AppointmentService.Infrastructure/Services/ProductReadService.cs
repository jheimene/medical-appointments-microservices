using OrderService.Application.Abstractions.Services;
using OrderService.Application.Dtos;
using System.Net.Http.Json;

namespace OrderService.Infrastructure.Services
{
    public class ProductReadService : IProductReadService
    {
        private readonly HttpClient _httpClient;

        public ProductReadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDataDto?> GetByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/api/products/{productId}", cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<ProductDataDto?>(cancellationToken);
        }
    }
}
