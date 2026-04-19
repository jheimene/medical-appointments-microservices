using Microsoft.EntityFrameworkCore;
using ProductService.Application.Abstractions.Persistence;
using ProductService.Domain.Categories.ValueObjects;
using ProductService.Infrastructure.Persistence.Contexts;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(Category category, CancellationToken ct = default)
        {
            _dbContext.Categories.Add(category);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsBySlugAsync(CategorySlug slug, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Category?> GetByIdAsync(CategoryId id, CancellationToken ct = default)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public Task<Category?> GetBySlugAsync(CategorySlug slug, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Category>> GetChildrenAsync(CategoryId parentId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(Category category)
        {
            throw new NotImplementedException();
        }

        public void Update(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
