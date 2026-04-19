using MediatR;
using PaymentService.Application.Dtos;
using PaymentService.Application.Handlers.Payments.Queries.GetByEventIdPayment;
using PaymentService.Application.Interfaces;

namespace PaymentService.Application.Handlers.Payments.Queries.GetByPaymentEventId
{
    public class GetPaymentByOrderIdHandler : IRequestHandler<GetPaymentByOrderId, PaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentByOrderIdHandler(IPaymentRepository paymentRepository)
        {
            this._paymentRepository = paymentRepository;
        }

        public async Task<PaymentResponse?> Handle(GetPaymentByOrderId request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetByOrderIdAsync(request.OrderId);
            if (payment is null) return null;

            return new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Method = payment.Method,
                CreatedDate = payment.CreatedDate,
                User = payment.User,
            };
        }
    }
}
