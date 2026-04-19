
using ProductService.Domain.Brands;
using ProductService.Domain.Brands.ValueObjects;

namespace ProductService.Application.Abstractions.Persistence
{
    public interface IBrandRepository
    {
        Task AddAsync(Brand brand, CancellationToken cancellationToken = default);
        Task<Brand?> GetByIdAsync(BrandId brandId, CancellationToken cancellationToken = default);
        Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        void Update(Brand brand);
    }
}
