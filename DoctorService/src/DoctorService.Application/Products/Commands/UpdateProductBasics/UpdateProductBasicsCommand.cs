using ErrorOr;
using MediatR;

namespace ProductService.Application.Products.Commands.UpdateProductBasics
{

    public sealed record UpdateProductBasicsCommand(
        Guid ProductId,
        string? Name,
        string? Slug,
        string? Description,
        Guid? BrandId,
        string? Model,
        IReadOnlyCollection<Guid>? CategoryIds,
        IReadOnlyCollection<string>? Tags,
        //IReadOnlyCollection<ProductAttributeDto>? Attributes,
        byte[]? RowVersion
    ) : IRequest<ErrorOr<Updated>>;

}
