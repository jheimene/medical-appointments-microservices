
namespace PaymentService.Application.Handlers.Payments.Commands.CreatePayment
{
    public sealed record CreatePaymentCommandResponse (
        int PaymentId,
        bool IsSuccess,
        string? TransactionId
    )
    {
    }
}
