using BffService.DTOs;

namespace BffService.Interfaces
{
    public interface IPaymentServiceClient
    {
        Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId, CancellationToken cancellationToken);
    }
}
