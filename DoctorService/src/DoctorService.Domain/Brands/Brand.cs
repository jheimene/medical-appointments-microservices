
using ProductService.Domain.Brands.ValueObjects;

namespace ProductService.Domain.Brands
{
    public sealed class Brand : AggregateRoot<BrandId, string>
    {
        public BrandName Name { get; private set; } = default!;
        public BrandCode Code { get; private set; } = default!;
        public BrandSlug Slug { get; private set; } = default!;
        public bool IsActive { get; private set; }

        private Brand() { }

        public static Brand Create(string name, string code, string slug, bool isActive = true)
        {
            var c = new Brand
            {
                Id = BrandId.NewId(),
                Name = BrandName.Create(name),
                Code = BrandCode.Create(code),
                Slug = BrandSlug.Create(slug),
                IsActive = isActive,
                CreatedAt = DateTime.Now
            };
            return c;
        }

        public void Rename(string newName)
        {
            Name = BrandName.Create(newName);
            Touch();
        }

        public void ChangeCode(string newCode)
        {
            Code = BrandCode.Create(newCode);
            Touch();

        }
        public void ChangeSlug(string newSlug)
        {
            Slug = BrandSlug.Create(newSlug);
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
