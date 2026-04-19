
namespace OrderService.Application.Dtos
{
    public sealed record PaymentResult
    {
        public int PaymentId { get; set; }
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; } = string.Empty;
    }
}
