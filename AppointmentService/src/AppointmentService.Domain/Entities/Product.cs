
using Events.Domain.Abstractions;

namespace OrderService.Domain.Entities
{
    public sealed class Product : BaseAuditableEntity
    {
        public Guid ProductId { get; private set; } = Guid.Empty;
        public string Name { get; private set; } = default!;

        private Product()
        {
        }

        public static Product Create(Guid productId, string name)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("El producto es obligatorio", nameof(productId));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre del producto es obligatorio", nameof(name));
            }
            return new Product
            {
                ProductId = productId,
                Name = name
            };
        }
    }
}
