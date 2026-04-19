using ErrorOr;
using MediatR;
using ProductService.Application.Products.Dtos;

namespace ProductService.Application.Products.Queries.GetProductImages
{
    public sealed record GetProductImagesByProductIdQuery(Guid ProductId) : IRequest<ErrorOr<IReadOnlyList<ProductImageDto>>>;
}
