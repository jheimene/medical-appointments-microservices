using BffService.DTOs;
using BffService.Interfaces;

namespace BffService.Clients
{
    public sealed class CustomerServiceClient : ICustomerServiceClient
    {
        private readonly HttpClient _httpClient;

        public CustomerServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<CustomerDto>(
                $"/api/customers/{customerId}",
                cancellationToken);
        }
    }
}
