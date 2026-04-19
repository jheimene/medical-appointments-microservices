
namespace OrderService.Application.Abstractions.Services
{
    public sealed class SequentialOrderNumberGenerator : IOrderNumberGenerator
    {
        public Task<string> GenerateAsync(CancellationToken cancellationToken)
        {
            var value = $"ORD-{DateTime.Now:yyyyMMddHHmmssfff}";
            return Task.FromResult(value);
        }
    }
}
