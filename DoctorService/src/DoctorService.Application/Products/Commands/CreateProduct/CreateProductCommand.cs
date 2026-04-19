using ErrorOr;
using MediatR;

namespace ProductService.Application.Products.Commands.CreateCustomer
{

    public sealed record CreateProductImageDto(
        string Url,
        int SortOrder,
        bool IsMain,
        string? AltText
    );

    public sealed record CreateVariantPriceDto(
        Guid PriceListId,
        decimal Amount,
        string Currency
    );

    public sealed record CreateProductCommand(
        string Name,
        string Slug,
        string Sku,
        string Currency,
        decimal Price,
        Guid ProductTypeId,
        Guid BrandId,
        string? Model,
        string? Description,
        List<Guid> CategoryIds,
        List<string>? Tags,
        Dictionary<string, string>? Attributes,
        List<CreateProductImageDto>? Images
    ) : IRequest<ErrorOr<Guid>>;

}
