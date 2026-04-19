
using ErrorOr;
using MediatR;
using ProductService.Domain.Categories;
using ProductService.Domain.Categories.ValueObjects;

namespace ProductService.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ErrorOr<Guid>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {

            var category = Category.Create(
                command.Name,
                command.Code,
                command.Slug,
                (command.ParentId is not null) ? new CategoryId(command.ParentId ?? Guid.Empty) : null
             );

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return category.Id.Value;
        }
    }
}
