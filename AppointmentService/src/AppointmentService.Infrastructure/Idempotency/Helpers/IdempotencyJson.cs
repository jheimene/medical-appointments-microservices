using System.Text.Json;

namespace OrderService.Infrastructure.Idempotency.Helpers
{
    public static class IdempotencyJson
    {
        public static readonly JsonSerializerOptions Default = new(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static string Serialize<T>(T value)
            => JsonSerializer.Serialize(value, Default);

        public static T? Deserialize<T>(string json)
            => JsonSerializer.Deserialize<T>(json, Default);
    }
}
