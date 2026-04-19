
namespace ProductService.Application.Brands.Dtos
{
    public sealed record BrandDto(
        Guid BrandId,
        string Code,
        string Name,
        string Slug,
        bool IsActive        
     ) 
    {
    }
}
