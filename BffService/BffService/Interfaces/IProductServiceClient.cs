using BffService.DTOs;

namespace BffService.Interfaces
{
    public interface IProductServiceClient
    {
        Task<PaymentDto?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken);
    }
}
