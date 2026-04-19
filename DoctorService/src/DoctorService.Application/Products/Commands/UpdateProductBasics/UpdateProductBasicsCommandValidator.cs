
using FluentValidation;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;

namespace ProductService.Application.Products.Commands.UpdateProductBasics
{
    public class UpdateProductBasicsCommandValidator : AbstractValidator<UpdateProductBasicsCommand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UpdateProductBasicsCommandValidator(IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }

        public UpdateProductBasicsCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("El producto no existe");
            RuleFor(x => x.Name).MaximumLength(200);
            RuleFor(x => x.Slug).MaximumLength(160);

            RuleFor(x => x.Description).MaximumLength(1000);

            RuleFor(x => x.BrandId)
                .MustAsync(async (brandId, cancellationToken) =>
                {
                    var brand = await _brandRepository!.GetByIdAsync(new BrandId(brandId ?? Guid.Empty), cancellationToken);
                    return brand != null;
                }).WithMessage("BrandId does not exist.");

            RuleFor(x => x.Model).MaximumLength(50);

            RuleForEach(x => x.CategoryIds)
                .NotEmpty().WithMessage("CategoryId cannot be empty.")
                .MustAsync(async (categoryId, cancellationToken) =>
                {
                    var category = await _categoryRepository!.GetByIdAsync(new CategoryId(categoryId), cancellationToken);
                    return category != null;
                }).WithMessage("CategoryId does not exist.");

            RuleForEach(x => x.Tags)
                .NotEmpty().WithMessage("Tag cannot be empty.");

            // RuleFor(RuleFor(x => x.Tags).NotNull().When(x => x.Tags != null))

        }
    }
}
