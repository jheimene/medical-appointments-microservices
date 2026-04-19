namespace OrderService.Application.Abstractions.Services
{
    public interface IOrderIdempotencyService
    {
        Task<IdempotencyBeginResult> BeginAsync(string idempotencyKey, string requestBody, CancellationToken cancellationToken);

        Task CompleteAsync(string idempotencyKey, string requestBody, int statusCode, string responseBody, string resourceId, CancellationToken cancellationToken);

        Task ReleaseAsync(string idempotencyKey, CancellationToken cancellationToken);
    }

    public sealed class IdempotencyBeginResult
    {
        public bool CanExecute { get; init; }
        public bool HasCachedResponse { get; init; }
        public int? CachedStatusCode { get; init; }
        public string? CachedResponseBody { get; init; }
        public string? ErrorMessage { get; init; }

        public static IdempotencyBeginResult Execute() =>
            new() { CanExecute = true };

        public static IdempotencyBeginResult Cached(int statusCode, string responseBody) =>
            new()
            {
                CanExecute = false,
                HasCachedResponse = true,
                CachedStatusCode = statusCode,
                CachedResponseBody = responseBody
            };

        public static IdempotencyBeginResult Error(string message) =>
            new()
            {
                CanExecute = false,
                HasCachedResponse = false,
                ErrorMessage = message
            };
    }
}
