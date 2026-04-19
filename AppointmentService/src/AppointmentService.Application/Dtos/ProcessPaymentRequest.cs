
namespace OrderService.Application.Dtos
{
    public sealed record ProcessPaymentRequest(
        string Provider,
        string Currency,
        decimal Amount,
        Guid OrderId,
        string OrderNumber,
        Guid CustomerId,
        string CustomerFullName,
        string User,
        string IdEmpotencyKey
    );
}
