using MediatR;

namespace PaymentService.Application.Handlers.Payments.Commands.CreatePayment;

public record CreatePaymentCommand(
    string Provider, 
    string Currency, 
    decimal Amount, 
    Guid OrderId,
    string? OrderNumber,
    Guid CustomerId,
    string? CustomerFullName,
    string User,
    string? IdempotencyKey
) : IRequest<CreatePaymentCommandResponse>;


