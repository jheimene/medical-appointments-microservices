
using OrderService.Application.Dtos;

namespace OrderService.Application.Abstractions.Services
{
    public interface IPaymentProcessService
    {
        Task<PaymentResult?> ProcessPaymentAsync(ProcessPaymentRequest processPayment, CancellationToken cancellation);

    }
}
