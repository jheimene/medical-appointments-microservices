using Mapster;
using ProductService.Application.Brands.Dtos;
using ProductService.Domain.Brands;

namespace ProductService.Application.Brands.Mappings
{
    public class BrandMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Brand, BrandDto>()
                .Map(dest => dest.BrandId, src => src.Id.Value)
                .Map(dest => dest.Code, src => src.Code.Value)
                .Map(dest => dest.Name, src => src.Name.Value)
                .Map(dest => dest.IsActive, src => src.IsActive);
        }
    }
}
