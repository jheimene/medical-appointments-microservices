using ErrorOr;
using MediatR;
using ProductService.Application.Products.Dtos;

namespace ProductService.Application.Products.Queries.GetProductImageById
{
    public sealed record GetProductImageByIdQuery(
        Guid ProductId,
        Guid ImageId
    ) : IRequest<ErrorOr<ProductImageDto>>;
}
