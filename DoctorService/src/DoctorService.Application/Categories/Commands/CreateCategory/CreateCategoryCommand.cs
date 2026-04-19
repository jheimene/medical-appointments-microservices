using ErrorOr;
using MediatR;

namespace ProductService.Application.Categories.Commands.CreateCategory
{
    public sealed record CreateCategoryCommand(
        string Name,
        string Code,
        string Slug,
        Guid? ParentId
    ) : IRequest<ErrorOr<Guid>>
    {
    }
}
