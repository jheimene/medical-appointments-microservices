using ErrorOr;
using MediatR;
using ProductService.Application.ProductsTypes.Mappings;
using ProductService.Domain.ProductsTypes.ValueObjects;

namespace ProductService.Application.ProductsTypes.Queries.GetByIdProductType
{
    public sealed class GetByIdProductTypeQueryHandler(
        IProductTypeRepository productTypeRepository
    ) : IRequestHandler<GetByIdProductTypeQuery, ErrorOr<GetByIdProductTypeQueryResponse>>
    {
        public async Task<ErrorOr<GetByIdProductTypeQueryResponse>> Handle(GetByIdProductTypeQuery request, CancellationToken cancellationToken)
        {
            var productType = await productTypeRepository.GetByIdAsync(new ProductTypeId(request.ProductTypeId), cancellationToken);
            if (productType is null) { return Error.NotFound("ProductType.NotFound", $"ProductType with ID {request.ProductTypeId} was not found."); }
            return productType.ToProductTypeQueryReponse();
        }
    }
}
