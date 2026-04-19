using ProductService.Domain.Categories.ValueObjects;

namespace ProductService.Domain.Products
{
    public sealed class ProductCategoryLink
    {
        public CategoryId CategoryId { get; private set; } = default!; 

        private ProductCategoryLink() { }

        public ProductCategoryLink(CategoryId categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
