using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using ProductService.Application.Products.Dtos;

namespace ProductService.Application.Products.Commands.PatchProduct
{
    public sealed record PatchProductCommand(
        Guid ProductId,
        JsonPatchDocument<ProductPatchDto> PatchDocument
        ) : IRequest<ErrorOr<PatchProductResponse>>
    {
    }
}
