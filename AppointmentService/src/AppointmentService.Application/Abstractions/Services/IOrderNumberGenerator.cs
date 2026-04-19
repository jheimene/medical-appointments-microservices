namespace OrderService.Application.Abstractions.Services
{
    public interface IOrderNumberGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
