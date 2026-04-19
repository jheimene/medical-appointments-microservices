using FluentValidation;
using ProductService.Application.Products.Dtos;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Commands.PatchProduct
{
    public sealed class ProductPatchDtoValidator : AbstractValidator<ProductPatchDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;

        public ProductPatchDtoValidator(IProductRepository productRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;

            //RuleFor(x => x.Name)
            //    //.NotEmpty().WithMessage("Name cannot be empty.")
            //    .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            //RuleFor(x => x.Description)
            //    .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            //RuleFor(x => x)
            //    .ChildRules(rules =>
            //    {
            //        rules.RuleFor(x => x.Slug).MaximumLength(160).WithMessage("Slug cannot exceed 160 characters.");
            //    });
            //    //.MustAsync(async (p, cancellationToken) =>
            //    //{
            //    //    var exists = await _productRepository.ExistsBySlugAsync(p.Slug!, new ProductId(p.ProductId), cancellationToken);                    
            //    //    return exists;
            //    //});

            //RuleFor(x => x)
            //     .ChildRules(rules =>
            //     {
            //         rules.RuleFor(x => x.BrandId)
            //             .MustAsync(async (brandId, cancellationToken) =>
            //             {
            //                 if (brandId == null) return true; // Skip validation if BrandId is not provided
            //                 var brand = await _brandRepository.GetByIdAsync(new BrandId(brandId.Value), cancellationToken);
            //                 return brand != null;
            //             }).WithMessage("BrandId does not exist.");
            //     });

            //RuleFor(x => x.Model)
            //  .MaximumLength(50).WithMessage("Model cannot exceed 50 characters.");

            //RuleFor(x => x.CategoryIds)
            //    .MustAsync(async (categoryIds, cancellationToken) =>
            //    {
            //        if (categoryIds == null || !categoryIds.Any()) return true; // Skip validation if no categories provided
            //        foreach (var categoryId in categoryIds)
            //        {
            //            var categoryExists = await _productRepository.ExistsCategoryByIdAsync(categoryId, cancellationToken);
            //            if (!categoryExists) return false;
            //        }
            //        return true;
            //    }).WithMessage("One or more CategoryIds do not exist.");

          
        }
    }
}
