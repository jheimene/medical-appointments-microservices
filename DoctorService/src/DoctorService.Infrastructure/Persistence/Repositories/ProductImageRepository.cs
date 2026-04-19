using Microsoft.EntityFrameworkCore;
using ProductService.Application.Abstractions.Persistence;
using ProductService.Domain.Products.Enums;
using ProductService.Domain.Products.ValueObjects;
using ProductService.Infrastructure.Persistence.Contexts;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    public class ProductImageRepository(ApplicationDbContext dbContext) : IProductImageRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public Task AddAsync(ProductImage image, CancellationToken cancellationToken = default) => _dbContext.ProductImages.AddAsync(image, cancellationToken).AsTask();

        public Task<int> CountActiveAsync(ProductId productId, CancellationToken cancellationToken = default)
            => _dbContext.ProductImages.CountAsync(
                x => x.ProductId == productId && x.Status == ProductImageStatus.Active,
                cancellationToken);

        public async Task<int> GetNextSortOrderAsync(ProductId productId, CancellationToken cancellationToken = default)
        {
            var max = await _dbContext.ProductImages
                .Where(x => x.ProductId == productId && x.Status == ProductImageStatus.Active)
                .Select(x => (int?)x.SortOrder)
                .MaxAsync(cancellationToken);

            return (max ?? 0) + 1;
        }

        public Task<ProductImage?> GetActiveByIdAsync(ProductId productId, Guid imageId, CancellationToken cancellationToken = default)
            => _dbContext.ProductImages.FirstOrDefaultAsync( 
                    x => x.ProductId == productId &&
                    x.Id == imageId &&
                    x.Status == ProductImageStatus.Active,
                    cancellationToken);

        public Task<ProductImage?> GetMainAsync(ProductId productId, CancellationToken cancellationToken = default)
        => _dbContext.ProductImages.FirstOrDefaultAsync(
            x => x.ProductId == productId &&
                 x.Status == ProductImageStatus.Active &&
                 x.IsMain,
            cancellationToken);

        public Task<List<ProductImage>> GetActiveListAsync(ProductId productId, CancellationToken cancellationToken = default)
            => _dbContext.ProductImages
                .Where(x => x.ProductId == productId && x.Status == ProductImageStatus.Active)
                .ToListAsync(cancellationToken);




        public async Task<IReadOnlyList<ProductImage>> GetByProductAsync(ProductId productId, CancellationToken cancellationToken = default)
            => await _dbContext.ProductImages
                .Where(x => x.ProductId == productId && x.Status == ProductImageStatus.Active)
                .OrderByDescending(x => x.IsMain)
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<ProductImage?> GetByIdAsync(ProductId productId, Guid imageId, CancellationToken cancellationToken = default)
            => await _dbContext.ProductImages
                .FirstOrDefaultAsync(x =>
                    x.ProductId == productId &&
                    x.Id == imageId &&
                    x.Status == ProductImageStatus.Active,
                    cancellationToken);

    }
}
