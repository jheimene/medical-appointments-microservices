
using ProductService.Domain.ProductsTypes;
using ProductService.Domain.ProductsTypes.ValueObjects;

namespace ProductService.Application.Abstractions.Persistence
{
    public interface IProductTypeRepository
    {
        Task AddAsync(ProductType productType, CancellationToken cancellation = default);
        Task<ProductType?> GetByIdAsync(ProductTypeId productTypeId, CancellationToken cancellation = default);
        Task<ProductType?> GetByNameAsync(string name, CancellationToken cancellation = default);
        void Update(ProductType productType);
    }
}
