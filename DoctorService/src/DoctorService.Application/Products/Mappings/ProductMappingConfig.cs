using Mapster;
using ProductService.Application.Products.Dtos;
using ProductService.Domain.Products;
using ProductService.Domain.Products.Enums;

namespace ProductService.Application.Products.Mappings
{
    public class ProductMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Product, ProductDto>()
                .Map(dest => dest.ProductId, src => src.Id.Value)
                .Map(dest => dest.Name, src => src.Name.Value)
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.IsActive, src => (src.Status == ProductStatus.Active))
                .Map(dest => dest.Currency, src => src.Price.Currency.Code)
                .Map(dest => dest.Price, src => src.Price.Amount);
        }
    }
}
