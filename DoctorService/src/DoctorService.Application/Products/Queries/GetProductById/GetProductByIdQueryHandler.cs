
using ErrorOr;
using Mapster;
using MediatR;
using ProductService.Application.Products.Dtos;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Queries.GetByIdProduct
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetByIdCategoryQuery, ErrorOr<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ErrorOr<ProductDto>> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(new ProductId(request.ProductId));
            if (product is null) return Error.NotFound("Product.NotFound", $"Product with ID {request.ProductId} was not found.");

            return product.Adapt<ProductDto>();
        }
    }
}
