using ProductService.Domain.Cat.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;

namespace ProductService.Domain.Categories
{
    public sealed class Category : AggregateRoot<CategoryId, string>
    {
        public CategoryName Name { get; private set; } = default!;
        public CategoryCode Code { get; private set; } = default!;
        public CategorySlug Slug { get; private set; } = default!;
        public CategoryId? ParentId { get; private set; }
        public bool IsActive { get; private set; }

        public Category? Parent { get; set; }
        private readonly List<Category> _children = new();
        public IReadOnlyCollection<Category> Children => _children;

        private Category() { }

        public static Category Create(string name, string code, string slug, CategoryId? parentId = null, bool isActive = true)
        {
            var c = new Category
            {
                Id = CategoryId.NewId(),
                Name = CategoryName.Create(name),
                Code = CategoryCode.Create(code),
                Slug = CategorySlug.Create(slug),
                ParentId = parentId,
                IsActive = isActive,
                CreatedAt = DateTime.Now
            };

            return c;
        }

        public void Rename(string newName)
        {
            Name = CategoryName.Create(newName);
            Touch();
        }

        public void ChangeCode(string newCode)
        {
            Code = CategoryCode.Create(newCode);
            Touch();
        }

        public void ChangeSlug(string newSlug)
        {
            Slug = CategorySlug.Create(newSlug);
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

        /// <summary>
        /// Mover categoría dentro del árbol.
        /// isMoveAllowed debe validar:
        /// - no mover a sí misma
        /// - no mover a un hijo (ciclo)
        /// - (opcional) límites de profundidad
        /// </summary>
        public void Move(CategoryId? newParentId, Func<CategoryId, CategoryId?, bool> isMoveAllowed)
        {
            if (!isMoveAllowed(Id, newParentId))
                throw new BusinessRuleViolationException("", "Movimiento de categoría inválido: generaría un ciclo o jerarquía inconsistente.");

            ParentId = newParentId;
            Touch();
        }

        private void Touch() => LastModifiedAt = DateTime.Now;
    }
}
