using ProductService.Application.Products.Dtos;

namespace ProductService.Application.Abstractions.Persistence
{
    public interface IProductImageReadRepository
    {
        Task<IReadOnlyList<ProductImageDto>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<ProductImageDto?> GetByIdAsync(Guid productId, Guid imageId, CancellationToken cancellationToken = default);
    }
}
