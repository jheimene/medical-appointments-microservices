using ErrorOr;
using MediatR;
using ProductService.Application.Common.Models;
using ProductService.Application.Products.Dtos;

namespace ProductService.Application.Products.Commands.UploadProductImage
{
    public sealed record UploadProductImageCommand(
        Guid ProductId,
        FileUploadData File,
        bool IsMain,
        int? SortOrder
    ) : IRequest<ErrorOr<ProductImageDto>>;
}
