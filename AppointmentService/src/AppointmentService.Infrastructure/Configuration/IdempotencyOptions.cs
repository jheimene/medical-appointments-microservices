namespace OrderService.Infrastructure.Configuration
{
    public sealed class IdempotencyOptions
    {
        public int ProcessingTtlMinutes { get; set; } = 5;
        public int CompletedTtlHours { get; set; } = 24;
    }
}
