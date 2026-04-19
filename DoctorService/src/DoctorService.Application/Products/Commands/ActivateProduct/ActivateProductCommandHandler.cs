using ErrorOr;
using MediatR;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Application.Products.Commands.ActivateProduct
{
    public sealed class ActivateProductCommandHandler
        : IRequestHandler<ActivateProductCommand, ErrorOr<Updated>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateProductCommandHandler(IProductRepository products, IUnitOfWork unitOfWork)
            => (_productRepository, _unitOfWork) = (products, unitOfWork);

        public async Task<ErrorOr<Updated>> Handle(ActivateProductCommand req, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(new ProductId(req.ProductId), cancellationToken);
            if (product is null) return Error.NotFound("product.not_found", "Producto no existe.");

            product.Activate();

            //_productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Updated;
        }
    }
}
