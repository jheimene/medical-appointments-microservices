using BffService.DTOs;

namespace BffService.Interfaces
{
    public interface IDispatchServiceClient
    {
        Task<ShipmentDto?> GetShipmentByIdAsync(Guid shipmentId, CancellationToken cancellationToken);
    }
}
