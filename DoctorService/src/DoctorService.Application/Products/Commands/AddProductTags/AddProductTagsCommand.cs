
using ErrorOr;
using MediatR;

namespace ProductService.Application.Products.Commands.AddProductTags
{
    public sealed record AddProductTagsCommand(
        Guid ProductId,
        IReadOnlyCollection<string> Tags
    ) : IRequest<ErrorOr<Updated>>
    {
    }
}
