
using OrderService.Application.Dtos;

namespace OrderService.Application.Abstractions.Services
{
    public interface IProductReadService
    {
        Task<ProductDataDto?> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    }
}
