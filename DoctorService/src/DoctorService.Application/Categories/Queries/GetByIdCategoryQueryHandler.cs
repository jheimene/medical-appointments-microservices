
using ErrorOr;
using Mapster;
using MediatR;
using ProductService.Domain.Categories.ValueObjects;

namespace ProductService.Application.Categories.Queries
{
    public class GetByIdCategoryQueryHandler : IRequestHandler<GetByIdCategoryQuery, ErrorOr<GetByIdCategoryQueryReponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetByIdCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ErrorOr<GetByIdCategoryQueryReponse>> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(new CategoryId(request.CategoryId));

            if (category is null) return Error.NotFound("Category.NotFound", $"Category with ID {request.CategoryId} was not found.");

            return new GetByIdCategoryQueryReponse(
            
                CategoryId: category.Id.Value,
                Name: category.Name.Value,
                Slug: category.Slug.Value,
                ParentId: category.ParentId?.Value,
                IsActive: category.IsActive
            );

            //return category.Adapt<GetByIdCategoryQueryReponse>();
        }
    }
}
