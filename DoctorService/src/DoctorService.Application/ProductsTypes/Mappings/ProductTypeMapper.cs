
using ProductService.Application.ProductsTypes.Queries.GetByIdProductType;
using ProductService.Domain.ProductsTypes;

namespace ProductService.Application.ProductsTypes.Mappings
{
    public static class ProductTypeMapper
    {
        public static GetByIdProductTypeQueryResponse ToProductTypeQueryReponse(this ProductType productType)
        {
            return new GetByIdProductTypeQueryResponse(
                productType.Id.Value,
                productType.Code.Value,
                productType.Name.Value,
                productType.IsActive
            );
        }


    }
}
