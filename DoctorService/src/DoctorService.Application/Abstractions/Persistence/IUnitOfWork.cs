namespace ProductService.Application.Abstractions.Persistence
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
