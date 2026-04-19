using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;
using ProductService.Domain.Products;
using ProductService.Domain.Products.ValueObjects;
using ProductService.Domain.ProductsTypes.ValueObjects;

namespace ProductService.Application.Products.Commands.CreateCustomer
{
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ErrorOr<Guid>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfwork, ILogger<CreateProductCommandHandler> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _unitOfWork = unitOfwork ?? throw new ArgumentNullException(nameof(unitOfwork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ErrorOr<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            // 1. Crear el producto (Agregado)
            //var product = Product.Create(
            //    name: ProductName.Create(command.Name),
            //    slug: command.Slug,
            //    sku: command.Sku,
            //    currency: command.Currency,
            //    price: command.Price,
            //    brandId: new BrandId(command.BrandId),
            //    model: command.Model,
            //    description: command.Description
            // );

            var product = Product.Create(
               name: ProductName.Create(command.Name),
               slug: Slug.Create(command.Slug),
               sku: Sku.Create(command.Sku),
               price: Money.Create(command.Price, Currency.Create(command.Currency)),
               productTypeId: new ProductTypeId(command.ProductTypeId),
               brandId: new BrandId(command.BrandId),
               model: command.Model//,
               //description: command.Description
            );
            product.SetDescription(command.Description);

            // 2) Categorías (N:N por ids)
            foreach (var id in command.CategoryIds.Distinct())
                product.AssignCategory(new CategoryId(id));

            // 3) Tags
            if (command.Tags is not null)
                foreach (var t in command.Tags.Where(x => !string.IsNullOrWhiteSpace(x)))
                    product.AddTag(t);

            // 4) Atributos (facetas de búsqueda)
            if (command.Attributes is not null)
            {
                foreach (var kv in command.Attributes)
                    product.AddOrUpdateAttribute(ProductAttribute.Create(kv.Key, kv.Value, isFilterable: true));
            }

            ////// 5) Imágenes a nivel producto
            //if (command.Images is not null && command.Images.Count > 0)
            //{
            //    var imgs = command.Images.Select(i => ProductImage.Create(i.Url, i.SortOrder, i.IsMain, i.AltText));
            //    product.SetImages(imgs);
            //}

            product.Confirm();

            // 6) Persistir el agregado
            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id.Value;
        }
    }
}
