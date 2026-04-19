
using ErrorOr;
using MediatR;

namespace ProductService.Application.ProductsTypes.Queries.GetByIdProductType
{
    public sealed record GetByIdProductTypeQuery(Guid ProductTypeId) : IRequest<ErrorOr<GetByIdProductTypeQueryResponse>>
    {
    }
}
