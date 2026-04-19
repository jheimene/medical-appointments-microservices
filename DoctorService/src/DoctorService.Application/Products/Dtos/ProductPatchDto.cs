using ProductService.Domain.Products;

namespace ProductService.Application.Products.Dtos
{
    public sealed class ProductPatchDto
    {
        //public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public Guid? BrandId { get; set; }
        public string? Model { get; set; }

        public List<Guid>? CategoryIds { get; set; }
        public List<string>? Tags { get; set; }

        public static ProductPatchDto FromDomain(Product product)
        {
            return new ProductPatchDto
            {
                //ProductId = product.Id.Value,
                Name = product.Name.Value,
                Slug = product.Slug.Value,
                Description = product.Description,
                BrandId = product.BrandId.Value,
                Model = product.Model?.Value,
                CategoryIds = product.CategoryIds.Select(c => c.CategoryId.Value).ToList(),
                Tags = product.Tags.Select(t => t.Value).ToList()
            };

        }
    }
}
