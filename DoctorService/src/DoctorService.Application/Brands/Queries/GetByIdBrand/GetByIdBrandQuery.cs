
using ErrorOr;
using MediatR;
using ProductService.Application.Brands.Dtos;

namespace ProductService.Application.Brands.Queries.GetByIdBrand
{
    public sealed record GetByIdBrandQuery(Guid BrandId) : IRequest<ErrorOr<BrandDto>>
    {
    }
}
