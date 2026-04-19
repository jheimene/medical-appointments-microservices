
using ErrorOr;
using MediatR;
using ProductService.Application.Products.Dtos;
using ProductService.Application.Products.Mappings;
using ProductService.Application.Products.Queries.GetProductImages;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Queries.GetProductImagesByProductId
{
    public sealed class GetProductImagesByProductIdQueryHandler(IProductImageRepository productImageRepository) 
        : IRequestHandler<GetProductImagesByProductIdQuery, ErrorOr<IReadOnlyList<ProductImageDto>>>
    {
        private readonly IProductImageRepository _productImageRepository = productImageRepository;

        public async Task<ErrorOr<IReadOnlyList<ProductImageDto>>> Handle(GetProductImagesByProductIdQuery request, CancellationToken cancellationToken)
        {
            var productImageList = await _productImageRepository.GetByProductAsync(new ProductId(request.ProductId), cancellationToken);

            return productImageList.Select(p => p.ToProductImageDto()).ToList();
        }
     
    }
}
