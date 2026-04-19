using ErrorOr;
using MediatR;
using ProductService.Application.Products.Dtos;
using ProductService.Application.Products.Mappings;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Queries.GetProductImageById
{
    public sealed class GetProductImageByIdQueryHandler(IProductImageRepository productImageRepository) 
        : IRequestHandler<GetProductImageByIdQuery, ErrorOr<ProductImageDto>>
    {
        private readonly IProductImageRepository _productImageRepository = productImageRepository;

        public async Task<ErrorOr<ProductImageDto>> Handle(GetProductImageByIdQuery request, CancellationToken cancellationToken)
        {
            // Validar existencia
            
            var productImage = await _productImageRepository.GetByIdAsync(new ProductId(request.ProductId), request.ImageId, cancellationToken);

            if (productImage is null) {
                return Error.NotFound("product_image.not_found", $"La imagen del producto {request.ProductId} no ha sido encontrada");
            }

            return productImage.ToProductImageDto();
        }
    }
}
