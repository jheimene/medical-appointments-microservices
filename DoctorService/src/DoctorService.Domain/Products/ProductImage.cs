using ProductService.Domain.Products.Enums;
using ProductService.Domain.Products.ValueObjects;

namespace ProductService.Domain.Products
{
    public sealed class ProductImage :AuditableEntity<Guid, string>
    {
        public ProductId  ProductId { get; private set; }
        public ImageUrl Url { get; private set; } = default!;
        public string ObjectKey { get; private set; } = string.Empty;
        public string OriginalFileName { get; private set; } = string.Empty;
        public string ContentType { get; private set; } = string.Empty;
        public long SizeBytes { get; private set; }
        public int SortOrder { get; private set; }
        public bool IsMain { get; private set; }
        public string? AltText { get; private set;  }
        public ProductImageStatus Status { get; set; }

        //private ProductImage(ImageUrl url, int sortOrder, bool isMain, string? altText)
        //{
        //    Url = url;
        //    SortOrder = sortOrder;
        //    IsMain = isMain;
        //    AltText = string.IsNullOrWhiteSpace(altText) ? null : altText.Trim();
        //}
        //public static ProductImage Create(string url, int sortOrder, bool isMain = false, string? altText = null)
        //{
        //    Guard.Against(sortOrder < 0, "", "SortOrder no puede ser negativo.");
        //    if (altText is not null) Guard.Against(altText.Length > 160, "", "AltText excede 160 caracteres.");

        //    return new ProductImage(ImageUrl.Create(url), sortOrder, isMain, altText);
        //}

        private ProductImage() { }

        public static ProductImage Create(
            Guid ProductImageId,
            ProductId productId,
            (string objectKey, string originalFileName, string contentType, long sizeBytes) imageObject,
            ImageUrl url,
            int sortOrder,
            bool isMain,
            string? altText
         )
        {
            // Validaciones

            return new ProductImage()
            {
                Id = ProductImageId,
                ProductId = productId,
                ObjectKey = imageObject.objectKey,
                OriginalFileName = imageObject.originalFileName,
                ContentType = imageObject.contentType,
                SizeBytes = imageObject.sizeBytes,
                Url = url,
                SortOrder = sortOrder,
                IsMain = isMain,
                AltText = altText
            };
        }

        public void SetAsMain() => IsMain = true;
        public void UnsetAsMain() => IsMain = false;
        public void MarkAsDeleted()
        {
            IsMain = false;
            Status = ProductImageStatus.Deleted;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = "System";
        }

    }
}
