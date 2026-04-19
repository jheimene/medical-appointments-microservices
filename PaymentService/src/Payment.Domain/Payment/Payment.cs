using PaymentService.Domain.Common;
using PaymentService.Domain.Payment.Enums;

namespace PaymentService.Domain.Payment;

public class Payment : BaseAuditableEntity
{
    public int PaymentId { get; private set; } = default!;
    public PaymentMethod Provider { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public Guid OrderId { get; private set; } = Guid.Empty;
    public string? OrderNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; } = Guid.Empty;
    public string? CustomerFullName { get; private set; } = string.Empty;
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;

    public string? ExternalPaymentId { get; private set; } = string.Empty;

    public string IdEmpotencyKey { get; private set; } = string.Empty;

    private Payment() { }
    

    public static Payment Create(
        PaymentMethod provider, 
        string currency, 
        decimal amount, 
        Guid orderId,
        string? orderNumber, 
        Guid customerId,
        string? customerName,
        string idempotencyKey,
        string user
    )
    {
        if (amount <= 0) throw new InvalidOperationException("El monto debe ser mayor a cero.");

        return new Payment
        {
            Provider = provider,
            Currency = currency,
            Amount = amount,
            OrderId = orderId,
            OrderNumber = orderNumber,
            CustomerId = customerId,
            CustomerFullName = customerName,
            Status = PaymentStatus.Pending,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = user,
            ModifiedDate = DateTime.UtcNow,
            ModifiedBy = user
        };
    }



}
