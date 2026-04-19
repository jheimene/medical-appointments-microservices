using ErrorOr;
using FluentValidation;
using MediatR;
using ProductService.Application.Products.Dtos;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;
using ProductService.Domain.Products;
using ProductService.Domain.Products.ValueObjects;
using System.Windows.Input;

namespace ProductService.Application.Products.Commands.PatchProduct
{
    public sealed class PatchProductCommandHandler : IRequestHandler<PatchProductCommand, ErrorOr<PatchProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProductPatchDto> _validator;

        public PatchProductCommandHandler(IProductRepository productRepository, IBrandRepository brandRepository, IUnitOfWork unitOfWork, IValidator<ProductPatchDto> validator)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<ErrorOr<PatchProductResponse>> Handle(PatchProductCommand request, CancellationToken cancellationToken)
        {
            var productId = new ProductId(request.ProductId);

            var product = await _productRepository.GetByIdForUpdateAsync(productId);

            if (product == null) { return Error.NotFound("Product.NotFound", $"Product with ID {request.ProductId} not found."); }

            // 1. Map the product to a DTO for patching
            var productDto = ProductPatchDto.FromDomain(product);

            // 2. Determine which properties are being patched
            var touchedPatch = request.PatchDocument.Operations
                .Select(o => NormalizePath(o.path))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 3. Validate the patch document against the touched properties
            var patchErrors = new List<string>();

            if (touchedPatch.Contains("/name") && string.IsNullOrWhiteSpace(productDto.Name)) { patchErrors.Add("Name cannot be empty."); }
            if (touchedPatch.Contains("/slug") && string.IsNullOrWhiteSpace(productDto.Slug)) { patchErrors.Add("Slug cannot be empty."); }


            request.PatchDocument.ApplyTo(productDto, error =>
            {
                patchErrors.Add(error.ErrorMessage);
            });

            if (patchErrors.Any())
            {
                return Error.Validation("Product.PatchFailed", string.Join("; ", patchErrors));
            }

            // 4. Apply the patch to the DTO
            var validation = await _validator.ValidateAsync(productDto, cancellationToken);
            if (validation != null && validation.Errors.Any())
            {
                var validationErrors = validation.Errors.Select(e => e.ErrorMessage);
                return Error.Validation("Product.PatchFailed", string.Join("; ", validationErrors));
            }

            if (touchedPatch.Contains("/slug"))
            {
                var slugExists = await _productRepository.ExistsBySlugAsync(Slug.Create(productDto.Slug!), productId, cancellationToken);
                if (slugExists) { return Error.Conflict("Product.PatchFailed", "Slug already exists."); }
            }

            if (touchedPatch.Contains("/brandid"))
            {
                var brand = await _brandRepository.GetByIdAsync(new BrandId(productDto.BrandId ?? Guid.Empty), cancellationToken);
                if (brand is null) return Error.NotFound("brand.not_found", "Marca no existe.");
            }

            // 5. Apply the changes to the domain entity
            await ApplyPatch(product, productDto, touchedPatch);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PatchProductResponse(productId.Value);


        }

        private static async Task ApplyPatch(Product product, ProductPatchDto productDto, HashSet<string> touchedPaths)
        {
            //bool touchesBasic =
            //     touchedPaths.Contains("name") ||
            //     touchedPaths.Contains("slug") ||
            //     touchedPaths.Contains("description") ||
            //     touchedPaths.Contains("brandId") ||
            //     touchedPaths.Contains("model");

            // if (touchesBasic)
            // {

            // }

            bool touchesName = touchedPaths.Contains("/name");
            if (touchesName) { product.Rename(productDto.Name!); }

            bool touchesSlug = touchedPaths.Contains("/slug");
            if (touchesSlug && !string.IsNullOrWhiteSpace(productDto.Slug)) { product.ChangeSlug(productDto.Slug!); }

            bool touchesDescription = touchedPaths.Contains("/description");
            if (touchesDescription) { product.SetDescription(productDto.Description); }

            bool touchesBrandOrModel = touchedPaths.Contains("/brandid") || touchedPaths.Contains("/model");
            if (touchesBrandOrModel) { product.SetBrand(new BrandId(productDto.BrandId!.Value), productDto.Model); }

            if (touchedPaths.Contains("/categoryids"))
            {
                var existingCategoryIds = product.CategoryIds.Select(c => c.CategoryId).ToHashSet();
                var newCategoryIds = productDto.CategoryIds?.Select(id => new CategoryId(id)).ToHashSet() ?? new HashSet<CategoryId>();

                // Add new categories
                newCategoryIds.Except(existingCategoryIds).ToList().ForEach(id => product.AssignCategory(id));

                // Remove old categories
                existingCategoryIds.Except(newCategoryIds).ToList().ForEach(id => product.RemoveCategory(id));

                //product.ReplaceCategories(newCategoryIds);
            }
        }

        private static string NormalizePath(string? path)
        {
            //return path.Trim('/').Split('/')[0];
            return (path ?? string.Empty).Trim().ToLowerInvariant();
        }
    }
}
