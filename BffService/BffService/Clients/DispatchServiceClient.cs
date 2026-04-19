
using BffService.DTOs;
using BffService.Interfaces;

namespace BffService.Clients
{
    public sealed class DispatchServiceClient : IDispatchServiceClient
    {
        private readonly HttpClient _httpClient;

        public DispatchServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ShipmentDto?> GetShipmentByIdAsync(Guid shipmentId, CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<ShipmentDto?>(
                $"/api/dispatch/{shipmentId}/dispatch",
                cancellationToken);
        }
    }
}