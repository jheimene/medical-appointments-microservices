using ProductService.Application.Products.Dtos;
using ProductService.Domain.Products;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Abstractions.Persistence
{
    public interface IProductImageRepository
    {
        Task AddAsync(ProductImage image, CancellationToken cancellationToken = default);
        Task<int> CountActiveAsync(ProductId productId, CancellationToken cancellationToken = default);
        Task<int> GetNextSortOrderAsync(ProductId productId, CancellationToken cancellationToken = default);
        Task<ProductImage?> GetActiveByIdAsync(ProductId productId, Guid imageId, CancellationToken cancellationToken = default);
        Task<ProductImage?> GetMainAsync(ProductId productId, CancellationToken cancellationToken = default);
        Task<List<ProductImage>> GetActiveListAsync(ProductId productId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProductImage>> GetByProductAsync(ProductId productId, CancellationToken cancellationToken = default);
        Task<ProductImage?> GetByIdAsync(ProductId productId, Guid imageId, CancellationToken cancellationToken = default);
    }
}
