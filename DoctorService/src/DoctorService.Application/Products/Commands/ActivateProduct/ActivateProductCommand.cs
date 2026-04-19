using ErrorOr;
using MediatR;

namespace ProductService.Application.Products.Commands.ActivateProduct
{
    public sealed record ActivateProductCommand(Guid ProductId) : IRequest<ErrorOr<Updated>>;
}
