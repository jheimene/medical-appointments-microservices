using ErrorOr;
using Mapster;
using MediatR;
using ProductService.Application.Brands.Dtos;
using ProductService.Domain.Brands.ValueObjects;
using System.Reflection;

namespace ProductService.Application.Brands.Queries.GetByIdBrand
{
    public sealed class GetByIdBrandQueryHandler(IBrandRepository brandRepository) : IRequestHandler<GetByIdBrandQuery, ErrorOr<BrandDto>>
    {
        public async Task<ErrorOr<BrandDto>> Handle(GetByIdBrandQuery request, CancellationToken cancellationToken)
        {
            var brand = await brandRepository.GetByIdAsync(new BrandId(request.BrandId), cancellationToken);
            if (brand == null) { return Error.NotFound("Brand.NotFound", $"Brand with ID {request.BrandId} was not found."); }
            return brand.Adapt<BrandDto>();
        }
    }
}
