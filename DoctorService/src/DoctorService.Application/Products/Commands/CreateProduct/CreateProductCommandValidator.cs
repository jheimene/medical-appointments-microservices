using FluentValidation;
using ProductService.Application.Products.Commands.CreateCustomer;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Commands.CreateProduct
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CreateCustomerCommandValidator(IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Name)
                .NotEmpty().MaximumLength(200).WithMessage("Name cannot bet empty and must not exceed 200 characters.");

            RuleFor(x => x.Sku)
                .NotEmpty().MaximumLength(50).WithMessage("Sku cannot be empty and must not exceed 50 characters.")
                .MustAsync(async (sku, cancellationToken) =>
                {
                    return !await _productRepository.ExistsBySkuAsync(Sku.Create(sku), null, cancellationToken);
                }).WithMessage("Sku ya existe.");

            RuleFor(x => x.Slug)
                .NotEmpty().MaximumLength(160).WithMessage("Slug cannot be empty and must not exceed 160 characters.")
                .MustAsync(BeUniqueSlug).WithMessage("Slug ya existe");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("BrandId cannot by empty.")
                .MustAsync(async (brandId, cancellationToken) =>
                {
                    var brand = await _brandRepository!.GetByIdAsync(new BrandId(brandId), cancellationToken);
                    return brand != null;
                }).WithMessage("BrandId no existe.");


            RuleFor(x => x.CategoryIds)
                .NotNull().WithMessage("CategoryIds cannot be null.")
                .ForEach(categoryIdRule => categoryIdRule
                    .NotEmpty().WithMessage("CategoryId cannot be empty.")
                    .MustAsync(async (categoryId, cancellationToken) =>
                    {
                        var category = await _categoryRepository!.GetByIdAsync(new CategoryId(categoryId), cancellationToken);
                        return category != null;
                    }).WithMessage("CategoryId does not exist."));

            RuleForEach(x => x.Tags).NotEmpty().WithMessage("Tag cannot be empty.");

            //RuleFor(x => x.Tags)
            //   .NotNull().When(x => x.Tags != null)
            //   .ForEach(tagRule => tagRule
            //       .NotEmpty().WithMessage("Tag cannot be empty."));

        }

        private async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
        {
            return !await _productRepository.ExistsBySlugAsync(Slug.Create(slug), null, cancellationToken);
        }

    }
}
