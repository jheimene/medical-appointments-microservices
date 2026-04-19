using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductService.Application.Abstractions.Storage;
using ProductService.Application.Common.Models;
using ProductService.Application.Common.Options;
using ProductService.Application.Products.Dtos;
using ProductService.Domain.Products;
using ProductService.Domain.Products.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.Products.Commands.UploadProductImage
{
    public sealed class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommand, ErrorOr<ProductImageDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageRulesOptions _rules;

        public UploadProductImageCommandHandler(
        IProductRepository productRepository,
        IProductImageRepository productImageRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork,
        IOptions<ImageRulesOptions> rules)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
            _rules = rules.Value;
        }

        public async Task<ErrorOr<ProductImageDto>> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
        {
            if (!await _productRepository.ExistsByIdAsync(new ProductId(request.ProductId), cancellationToken))
                return Error.NotFound("product.not_found", $"El producto con id {request.ProductId} no ha sido encontrado.");

            if (request.File.Length <= 0)
                return Error.Failure("image.empty", "El archivo está vacío.");

            if (request.File.Length > _rules.MaxSizeBytes)
                return Error.Conflict("image.max_length", "La imagen excede el tamaño máximo permitido.");

            if (string.IsNullOrWhiteSpace(request.File.ContentType))
                return Error.Conflict("image.content_type_invalid", "El tipo de contenido es obligatorio.");

            if (!_rules.AllowedContentTypes.Contains(request.File.ContentType, StringComparer.OrdinalIgnoreCase))
                return Error.Conflict("image.type_unsupport", "Tipo de imagen no permitido.");

            var currentCount = await _productImageRepository.CountActiveAsync(new ProductId(request.ProductId), cancellationToken);
            if (currentCount >= _rules.MaxImagesPerProduct)
                return Error.Conflict("image.quantity", "Se alcanzó el máximo de imágenes permitidas para el producto.");

            var imageId = Guid.NewGuid();
            var safeFileName = Path.GetFileName(request.File.FileName);
            var extension = ResolveExtension(safeFileName, request.File.ContentType);
            var objectKey = BuildObjectKey(request.ProductId, imageId, extension);

            var metadata = new Dictionary<string, string>
            {
                ["product-id"] = request.ProductId.ToString(),
                ["image-id"] = imageId.ToString(),
                ["original-file-name"] = safeFileName
            };

            await _storageService.UploadAsync(
                request.File.Content,
                objectKey,
                request.File.ContentType,
                metadata,
                cancellationToken);

            var sortOrder = request.SortOrder ?? await _productImageRepository.GetNextSortOrderAsync(new ProductId(request.ProductId), cancellationToken);

            var hasMain = await _productImageRepository.GetMainAsync(new ProductId(request.ProductId), cancellationToken);
            var shouldBeMain = request.IsMain || hasMain is null;

            if (shouldBeMain && hasMain is not null)
                hasMain.UnsetAsMain();

            var image = ProductImage.Create(
                imageId,
                new ProductId(request.ProductId),
                (objectKey, safeFileName, request.File.ContentType, request.File.Length),
                ImageUrl.Create($"{objectKey}{extension}"),
                sortOrder,
                shouldBeMain, 
                "");

            await _productImageRepository.AddAsync(image, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ProductImageDto(
                image.Id,
                image.ProductId.Value,
                image.ObjectKey,
                image.OriginalFileName,
                image.ContentType,
                image.SizeBytes,
                image.IsMain,
                image.SortOrder,
                image.Status.ToString(),
                _storageService.GetReadUrl(image.ObjectKey),
                image.CreatedAt);
        }

        private void ValidateFile(FileUploadData file)
        {
            if (file.Length <= 0)
                throw new ValidationException("El archivo está vacío.");

            if (file.Length > _rules.MaxSizeBytes)
                throw new ValidationException("La imagen excede el tamaño máximo permitido.");

            if (string.IsNullOrWhiteSpace(file.ContentType))
                throw new ValidationException("El tipo de contenido es obligatorio.");

            if (!_rules.AllowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
                throw new ValidationException("Tipo de imagen no permitido.");
        }

        // $"products/{productId}/images/{DateTime.UtcNow:yyyy/MM}/{imageId}{extension}";
        private static string BuildObjectKey(Guid productId, Guid imageId, string extension) => $"products/{productId}/images/{imageId}{extension}";

        private static string ResolveExtension(string fileName, string contentType)
        {
            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(ext))
                return ext;

            return contentType.ToLowerInvariant() switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                _ => ".bin"
            };
        }
    }
}
