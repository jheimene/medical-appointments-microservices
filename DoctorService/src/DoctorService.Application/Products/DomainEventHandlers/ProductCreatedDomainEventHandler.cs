
using MediatR;
using ProductService.Application.Brands.Queries.GetByIdBrand;
using ProductService.Application.Categories.Queries;
using ProductService.Application.ProductsTypes.Queries.GetByIdProductType;
using ProductService.Domain.Products;
using ProductService.Domain.Products.Events;

namespace ProductService.Application.Products.DomainEventHandlers
{
    public class ProductCreatedDomainEventHandler : INotificationHandler<ProductCreatedDomainEvent>
    {
        private readonly IMediator _mediator;
        private readonly IProductSearchRepository _productSearchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCreatedDomainEventHandler(
            IMediator mediator,
            IProductSearchRepository productSearchRepository, 
            IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _productSearchRepository = productSearchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // Obtener el ProductType
            var productType = (await _mediator.Send(new GetByIdProductTypeQuery(notification.ProductTypeId), cancellationToken)).Value;

            // Obtener el Brand
            var brand = (await _mediator.Send(new GetByIdBrandQuery(notification.BrandId), cancellationToken)).Value;
            
            // Obtener los nombres de las categorias
            List<string> categories = [];
            if (notification.CategoryIds is not null)
            {
                foreach (var categoryId in notification.CategoryIds)
                {
                    var category = (await _mediator.Send(new GetByIdCategoryQuery(categoryId), cancellationToken)).Value;
                    if (category is not null)
                        categories.Add(category.Name);

                }
            }

            var productSearch = ProductSearch.Create(new ProductSearchCreationProps
            (
                notification.ProductId,
                notification.Name,
                notification.Slug,
                notification.Sku,
                notification.Currency,
                notification.Price,
                notification.Status,
                notification.Description,
                notification.ProductTypeId,
                productType.Name,
                notification.BrandId,
                brand.Name,
                notification.Model,
                notification.Tags,
                string.Join(", ", categories),
                ""
            ));

            await _productSearchRepository.AddAsync(productSearch, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
        }
    }
}
