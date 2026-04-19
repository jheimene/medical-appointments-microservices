using OrderService.Application.Abstractions.Services;
using OrderService.Application.Dtos;
using System.Net.Http.Json;

namespace OrderService.Infrastructure.Services
{
    public class CustomerReadService : ICustomerReadService
    {
        private readonly HttpClient _httpClient;

        public CustomerReadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerDataDto?> GetByIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/api/customers/{customerId}", cancellationToken);
            if (!response.IsSuccessStatusCode) return null;            
            return await response.Content.ReadFromJsonAsync<CustomerDataDto?>(cancellationToken);
        }
    }
}
