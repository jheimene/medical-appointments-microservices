using ProductService.Application.Products.Dtos;
using ProductService.Domain.Products;

namespace ProductService.Application.Products.Mappings
{
    public static class ProductMappings
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                ProductId = product.Id.Value,
                Name = product.Name.Value,
                Slug = product.Slug.Value,
                Description = product.Description,
                Status = product.Status.ToString(),
                //Brand = product.BrandId.Value,
                //Model = product.Model?.Value,
                //CategoryIds = product.CategoryIds.Select(c => c.CategoryId.Value).ToList(),
                //Tags = product.Tags.Select(t => t.Value).ToList(),
                //Attributes = product.Attributes.ToDictionary(a => a.Name, a => a.Value),
                //Images = product.Images.Select(i => new ProductImageDto
                //{
                //    Url = i.Url,
                //    SortOrder = i.SortOrder,
                //    IsMain = i.IsMain,
                //    AltText = i.AltText
                //}).ToList()
            };
        }
    }
}
