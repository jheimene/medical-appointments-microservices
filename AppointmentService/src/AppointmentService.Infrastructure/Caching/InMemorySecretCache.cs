
using System.Collections.Concurrent;

namespace OrderService.Infrastructure.Caching
{
    public sealed class InMemorySecretCache
    {
        private readonly TimeSpan _defaultTtl = TimeSpan.FromMinutes(5);
        private readonly ConcurrentDictionary<string, CacheEntry> _cache = new(StringComparer.OrdinalIgnoreCase);

        public bool TryGet(string cacheKey, out IReadOnlyDictionary<string, string>? data)
        {
            data = default!;

            if (!_cache.TryGetValue(cacheKey, out var entry))
                return false;

            if (entry.ExpiresAt <= DateTime.Now)
            {
                _cache.TryRemove(cacheKey, out _);
                return false;
            }

            data = entry.Data;
            return true;
        }

        public void Set(string cacheKey, IReadOnlyDictionary<string, string> data, TimeSpan? ttl)
        {
            var expiresAt = DateTime.Now.Add(ttl ?? _defaultTtl);
            _cache[cacheKey] = new CacheEntry(data, expiresAt);
        }
        public void Remove(string cacheKey) => _cache.TryRemove(cacheKey, out _);
        public void Clear() => _cache.Clear();
        public sealed record CacheEntry(IReadOnlyDictionary<string, string> Data, DateTime ExpiresAt);
    }
}
