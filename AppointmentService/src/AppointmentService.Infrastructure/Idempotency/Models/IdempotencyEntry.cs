namespace OrderService.Infrastructure.Idempotency.Models
{
    public sealed class IdempotencyEntry
    {
        public IdempotencyStatus State { get; set; } = default!; // PROCESSING | COMPLETED
        public string RequestHash { get; set; } = default!;
        public int? StatusCode { get; set; }
        public string? ResponseBody { get; set; }
        public string? ResourceId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? CompletedAtUtc { get; set; }
    }

    public enum IdempotencyStatus
    {
        Processing = 0,
        Completed = 1
    }
}
