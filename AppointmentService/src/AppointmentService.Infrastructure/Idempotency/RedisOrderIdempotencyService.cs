using Microsoft.Extensions.Options;
using OrderService.Application.Abstractions.Services;
using OrderService.Infrastructure.Configuration;
using OrderService.Infrastructure.Idempotency.Helpers;
using OrderService.Infrastructure.Idempotency.Models;
using StackExchange.Redis;

namespace OrderService.Infrastructure.Idempotency
{
    public sealed class RedisOrderIdempotencyService(IConnectionMultiplexer multiplexer, IOptions<IdempotencyOptions> options) : IOrderIdempotencyService
    {
        private readonly IDatabase _redis = multiplexer.GetDatabase();
        private readonly TimeSpan _processingTtl = TimeSpan.FromMinutes(options.Value.ProcessingTtlMinutes);
        private readonly TimeSpan _completedTtl = TimeSpan.FromHours(options.Value.CompletedTtlHours);

        public async Task<IdempotencyBeginResult> BeginAsync(string idempotencyKey, string requestBody, CancellationToken cancellationToken)
        {
            var redisKey = BuildKey(idempotencyKey);
            var requestHash = RequestHashHelper.ComputeSha256(requestBody);

            var processingEntry = new IdempotencyEntry
            {
                State = IdempotencyStatus.Processing,
                RequestHash = requestHash,
                CreatedAtUtc = DateTime.UtcNow
            };

            var processingJson = IdempotencyJson.Serialize(processingEntry);

            var acquired = await _redis.StringSetAsync(
                redisKey,
                processingJson,
                expiry: _processingTtl,
                when: When.NotExists
             );

            if (acquired)
                return IdempotencyBeginResult.Execute();

            var existingJson = await _redis.StringGetAsync(redisKey);

            if (existingJson.IsNullOrEmpty)
                return IdempotencyBeginResult.Error("no se pudo determinar el estado del idempotenia. Reintenta.");

            var existing = IdempotencyJson.Deserialize<IdempotencyEntry>(existingJson!);

            if (existing is null)
                return IdempotencyBeginResult.Error("El estado de idempotencia es inválido");

            if (!string.Equals(existing.RequestHash, requestHash, StringComparison.Ordinal))
                return IdempotencyBeginResult.Error("La misma Idempotency-Key fue enviada con un payload distinto.");

            if (existing.State == IdempotencyStatus.Completed)
                return IdempotencyBeginResult.Cached(
                    existing.StatusCode ?? 200,
                    existing.ResponseBody ?? "{}");

            if (existing.State == IdempotencyStatus.Processing)
                return IdempotencyBeginResult.Error("La solicitud ya está siendo procesada.");

            return IdempotencyBeginResult.Error("Estado de idempotencia no soportado.");
        }

        public async Task CompleteAsync(string idempotencyKey, string requestBody, int statusCode, string responseBody, string resourceId, CancellationToken cancellationToken)
        {
            var redisKey = BuildKey(idempotencyKey);
            var requestHash = RequestHashHelper.ComputeSha256(requestBody);

            var completedEntry = new IdempotencyEntry
            {
                State = IdempotencyStatus.Completed,
                RequestHash = requestHash,
                StatusCode = statusCode,
                ResponseBody = responseBody,
                ResourceId = resourceId,
                CreatedAtUtc = DateTime.UtcNow,
                CompletedAtUtc = DateTime.UtcNow
            };

            var completedJson = IdempotencyJson.Serialize(completedEntry);

            await _redis.StringSetAsync(
                redisKey,
                completedJson,
                expiry: _completedTtl,
                when: When.Always);
        }

        public async Task ReleaseAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            var redisKey = BuildKey(idempotencyKey);
            await _redis.KeyDeleteAsync(redisKey);
        }

        private static string BuildKey(string idempotencykey) => $"idem:orders:create:{idempotencykey}";
    }
}
