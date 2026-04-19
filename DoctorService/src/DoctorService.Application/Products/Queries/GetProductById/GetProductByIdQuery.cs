using ErrorOr;
using MediatR;
using ProductService.Application.Products.Dtos;

namespace ProductService.Application.Products.Queries.GetByIdProduct
{
    public sealed record GetByIdCategoryQuery(Guid ProductId) : IRequest<ErrorOr<ProductDto>> { }
}
