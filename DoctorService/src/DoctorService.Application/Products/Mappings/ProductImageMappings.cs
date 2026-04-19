
using ProductService.Application.Products.Dtos;
using ProductService.Domain.Products;

namespace ProductService.Application.Products.Mappings
{
    public static class ProductImageMappings
    {

        public static ProductImageDto ToProductImageDto(this ProductImage productImage)
        {
            return new ProductImageDto(
                productImage.Id,
                productImage.ProductId.Value,
                productImage.ObjectKey,
                productImage.OriginalFileName,
                productImage.ContentType,
                productImage.SizeBytes,
                productImage.IsMain,
                productImage.SortOrder,
                productImage.Status.ToString(),
                productImage.Url.Value,
                productImage.CreatedAt
                );
        }
    }
}
