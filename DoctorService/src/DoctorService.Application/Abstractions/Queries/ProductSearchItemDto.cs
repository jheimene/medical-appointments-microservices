namespace ProductService.Application.Abstractions.Queries
{
    public sealed record ProductSearchItemDto
    {

        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Slug { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }


        private ProductSearchItemDto()
        {
            
        }

        public ProductSearchItemDto(
            Guid productId,
            string name,
            string sku,
            string slug,
            string? brand,
            string? model,
            DateTime createdAt,
            bool isActive
        )
        {
            ProductId = productId;
            Name = name;
            Sku = sku;
            Slug = slug;
            Brand = brand;
            Model = model;
            CreatedAt = createdAt;
            IsActive = isActive;
        }

    }
}
