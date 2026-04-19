using PaymentService.Application.Dtos;
using PaymentService.Domain.Payment;
using PaymentService.Domain.Payment.Enums;

namespace PaymentService.Application.Interfaces;

public interface IPaymentRepository
{
    Task<int> CreateAsync(Payment payment);
    Task UpdateStatusAsync(int paymentId, PaymentStatus status, string externalPaymentId, string extra, string userIdModified, DateTime modifiedDate);
    Task<PaymentResponse?> GetByOrderIdAsync(Guid orderId);
    Task<IEnumerable<PaymentResponse>?> GetByCustomerIdAsync(Guid customerId);

}

