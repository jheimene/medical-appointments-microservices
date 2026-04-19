
using ErrorOr;
using MediatR;

namespace ProductService.Application.Categories.Queries
{
    public sealed record GetByIdCategoryQuery(Guid CategoryId) : IRequest<ErrorOr<GetByIdCategoryQueryReponse>>
    {
    }
}
