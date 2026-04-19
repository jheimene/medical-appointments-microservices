using ErrorOr;
using MediatR;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Commands.RemoveProductTags
{
    public sealed class RemoveProductTagsCommandHandler : IRequestHandler<RemoveProductTagsCommand, ErrorOr<Updated>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProductTagsCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Updated>> Handle(RemoveProductTagsCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(new ProductId(request.ProductId), cancellationToken);

            if (product == null)
            {
                return Error.NotFound("El producto no existe");
            }

            if (request.Tags == null || !request.Tags.Any())
            {
                return Error.Validation("No se han proporcionado etiquetas para eliminar");
            }

            request.Tags.ToList().ForEach(tag => product.RemoveTag(tag));
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Updated;
        }
    }
}
