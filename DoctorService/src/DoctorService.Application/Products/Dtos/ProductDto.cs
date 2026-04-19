namespace ProductService.Application.Products.Dtos
{
    public class ProductDto
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string? Description { get; init; } 
        public string Status { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string? Model { get; init; }
        public string Currency { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public bool IsActive { get; init; }

        //public IList<string> Categories { get; set; }

        //public ProductDto()
        //{
        //    Categories = new List<string>();
        //}

    }
}
