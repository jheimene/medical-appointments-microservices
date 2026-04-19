using ProductService.Domain.ProductsTypes.ValueObjects;

namespace ProductService.Domain.ProductsTypes
{
    public sealed class ProductType : AggregateRoot<ProductTypeId, string>
    {
        public ProductTypeName Name { get; private set; } = default!;
        public ProductTypeCode Code { get; private set; } = default!;
        public bool IsActive { get; private set; }


        private ProductType() { }

        public static ProductType Create(string name, string code, bool isActive = true)
        {
            var c = new ProductType
            {
                Id = ProductTypeId.NewId(),
                Name = ProductTypeName.Create(name),
                Code = ProductTypeCode.Create(code),
                IsActive = isActive,
                CreatedAt = DateTime.Now
            };

            return c;
        }

        public void Rename(string newName)
        {
            Name = ProductTypeName.Create(newName);
            Touch();
        }

        public void ChangeCode(string newCode)
        {
            Code = ProductTypeCode.Create(newCode);
            Touch();
        }

        public void Activate()
        {
            IsActive = true;
            Touch();
        }

        public void Deactivate()
        {
            IsActive = false;
            Touch();
        }

        private void Touch() => LastModifiedAt = DateTime.Now;
    }
}
