using ProductService.Domain.Products;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(ProductId productId, CancellationToken cancellationToken = default);
        Task<Product?> GetByIdForUpdateAsync(ProductId productId, CancellationToken cancellationToken = default);

        // Reglas de unicidad / lookup
        Task<bool> ExistsByIdAsync(ProductId productId, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySkuAsync(Sku sku, ProductId? excludeId, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(Slug slug, ProductId? excludeId, CancellationToken cancellationToken = default);

        // Persistencia del agregado
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        void Update(Product product);
        void Remove(Product product);
    }
}
