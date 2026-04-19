using ErrorOr;
using MediatR;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Commands.UpdateProductBasics
{
    public sealed class UpdateProductBasicsCommandHandler
     : IRequestHandler<UpdateProductBasicsCommand, ErrorOr<Updated>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductBasicsCommandHandler(IProductRepository productRepository, IBrandRepository brandRepository, IUnitOfWork unitOfWork)
            => (_productRepository, _brandRepository, _unitOfWork) = (productRepository, brandRepository, unitOfWork);

        public async Task<ErrorOr<Updated>> Handle(UpdateProductBasicsCommand commnad, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdForUpdateAsync(new ProductId(commnad.ProductId), cancellationToken);
            if (product is null) return Error.NotFound("product.not_found", "Producto no existe.");

            if (!string.IsNullOrWhiteSpace(commnad.Name)) product.Rename(commnad.Name);

            if (!string.IsNullOrWhiteSpace(commnad.Slug))
            {
                var slugExists = await _productRepository.ExistsBySlugAsync(Slug.Create(commnad.Slug), new ProductId(commnad.ProductId), cancellationToken);
                if (slugExists) { return Error.Conflict("product.slug_exists", "Slug ya existe."); }
                product.ChangeSlug(commnad.Slug);
            }
            if (commnad.Description is not null) product.SetDescription(commnad.Description);

            if (commnad.BrandId.HasValue)
            {
                var brand = await _brandRepository.GetByIdAsync(new BrandId(commnad.BrandId.Value), cancellationToken);
                if (brand is null) return Error.NotFound("brand.not_found", "Marca no existe.");
                product.SetBrand(new BrandId(commnad.BrandId.Value), commnad.Model);
            }
            
            if (!string.IsNullOrWhiteSpace(commnad.Model)) product.ChangeModel(commnad.Model);

            if (commnad.CategoryIds is not null) product.ReplaceCategories(commnad.CategoryIds!.Select(id => new CategoryId(id)));
            
            if (commnad.Tags is not null) product.ReplaceTags(commnad.Tags);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Updated;
        }
    }
}
