using ErrorOr;
using MediatR;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Commands.AddProductTags
{
    public sealed class AddProductTagsCommandHandler : IRequestHandler<AddProductTagsCommand, ErrorOr<Updated>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddProductTagsCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Updated>> Handle(AddProductTagsCommand request, CancellationToken cancellationToken)
        {
            var productId = new ProductId(request.ProductId);

            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return Error.NotFound("Product.NotFound", $"Product with ID {request.ProductId} was not found.");
            }

            if (request.Tags == null || !request.Tags.Any())
            {
                return Error.Validation("Product.Tags.Invalid", "At least one tag must be provided.");
            }

            request.Tags.ToList().ForEach(tag => product.AddTag(tag));
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Updated;
        }
    }
}
