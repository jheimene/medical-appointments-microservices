

namespace PaymentService.Application.Dtos
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public string User { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
