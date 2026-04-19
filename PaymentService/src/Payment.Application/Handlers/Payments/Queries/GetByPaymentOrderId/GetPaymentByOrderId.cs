using MediatR;
using PaymentService.Application.Dtos;

namespace PaymentService.Application.Handlers.Payments.Queries.GetByEventIdPayment
{
    public class GetPaymentByOrderId(Guid OrderId) : IRequest<PaymentResponse>
    {
        public Guid OrderId { get; } = OrderId;
    }
}
