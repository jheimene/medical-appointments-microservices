using ErrorOr;
using MediatR;

namespace ProductService.Application.Products.Commands.RemoveProductTags
{
    public sealed record RemoveProductTagsCommand(
        Guid ProductId,
        IReadOnlyCollection<string> Tags
    ) : IRequest<ErrorOr<Updated>>
    {
    }
}
