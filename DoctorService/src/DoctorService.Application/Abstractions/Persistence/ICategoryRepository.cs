using ProductService.Domain.Categories;
using ProductService.Domain.Categories.ValueObjects;


namespace ProductService.Application.Abstractions.Persistence
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(CategoryId id, CancellationToken ct = default);
        Task<Category?> GetBySlugAsync(CategorySlug slug, CancellationToken ct = default);
        Task<bool> ExistsBySlugAsync(CategorySlug slug, CancellationToken ct = default);

        Task AddAsync(Category category, CancellationToken ct = default);
        void Update(Category category);
        void Remove(Category category);

        // Útil para construir árbol o validar jerarquía (sin meterte a “search”)
        Task<IReadOnlyList<Category>> GetChildrenAsync(CategoryId parentId, CancellationToken ct = default);
        Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken ct = default);
    }
}
