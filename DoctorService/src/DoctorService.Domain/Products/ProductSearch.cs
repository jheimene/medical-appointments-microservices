namespace ProductService.Domain.Products
{
    public sealed class ProductSearch
    {
        public Guid ProductId { get; private set; } = Guid.Empty;
        public string Name { get; private set; } = default!;
        public string NameNormalized { get; private set; } = default!;
        public string Slug { get; private set; } = default!;
        public string Sku { get; private set; } = default!;
        public string Currency { get; private set; } = default!;
        public decimal Price { get; private set; } = decimal.Zero;
        public string? Description { get; private set; } = default!;
        public string Status { get; private set; } = default!;
        public Guid ProductTypeId { get; private set; } = Guid.Empty;
        public string ProductType { get; private set; } = default!;
        public Guid BrandId { get; private set; } = Guid.Empty;
        public string Brand { get; private set; } = default!;
        public string? Model { get; private set; } = default!;
        public string? ModelNormalized { get; private set; } = default!;
        public string? Tags { get; private set; } = default!;
        public string? Categories { get; private set; } = default!;
        public string? Attributes { get; private set; } = default!;
        public string? SearchDocument { get; private set; } = default!;
        public string? SearchDocumentNormalized { get; private set; } = default!;

        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; } 
        public DateTime LastUpdatedAt { get; private set; }

        private ProductSearch()
        {
            
        }

        public static ProductSearch Create(ProductSearchCreationProps props)
        {
            var searchDocument = $"{props.Name} {props.Sku} {props.ProductType} {props.Brand} {props.Model} {props.Tags} {props.Categories} {props.Attributes}".Trim();

            return new ProductSearch
            {
                ProductId = props.ProductId,
                Name = props.Name,
                NameNormalized = props.Name.Trim().ToUpperInvariant(),
                Slug = props.Slug,
                Sku = props.Sku,
                Currency = props.Currency,
                Price = props.Price,
                Status = props.Status,
                Description = props.Description,
                ProductTypeId = props.ProductTypeId,
                ProductType = props.ProductType,
                BrandId = props.BrandId,
                Brand = props.Brand,
                Model = props.Model,
                ModelNormalized = props.Model?.Trim().ToUpperInvariant(),
                Tags = props.Tags,
                Categories = props.Categories,
                Attributes = props.Attributes,
                SearchDocument = searchDocument,
                SearchDocumentNormalized = searchDocument.ToUpperInvariant(),
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                LastUpdatedAt = DateTime.Now
            };
        }
    }
}
