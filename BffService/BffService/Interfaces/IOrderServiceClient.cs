using BffService.DTOs;

namespace BffService.Interfaces
{
    public interface IOrderServiceClient
    {
        Task<OrderDto?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken);
    }
}
