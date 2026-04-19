using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Payment;
using PaymentService.Domain.Payment.Enums;

namespace PaymentService.Application.Handlers.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentCommandResponse>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentProviderFactory _factory;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IPaymentProviderFactory factory)
    {
        _paymentRepository = paymentRepository;
        _factory = factory;
    }

    public async Task<CreatePaymentCommandResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {

        var method = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), request.Provider, ignoreCase: true);

        var payment = Payment.Create(
            method, request.Currency, 
            request.Amount, 
            request.OrderId, 
            request.OrderNumber,
            request.CustomerId,
            request.CustomerFullName,
            request.IdempotencyKey ?? "",
            request.User
        );

        var paymentId = await _paymentRepository.CreateAsync(payment);

        // Sincrono
        var provider = _factory.GetProvider(request.Provider.ToLower());

        bool process = false;
        string? externalId = null; 
        try
        {
            externalId = await provider.ProccessPaymentAsync(request.Amount, request.Currency, request.User);

            await _paymentRepository.UpdateStatusAsync(paymentId, PaymentStatus.Succeeded, externalId, "OK", request.User, DateTime.UtcNow);
            process = true;
        }
        catch (Exception ex)
        {
            await _paymentRepository.UpdateStatusAsync(paymentId, PaymentStatus.Failed, "", $"Error ... {ex.Message}", request.User, DateTime.UtcNow);
            process = false;
        }


        return new CreatePaymentCommandResponse(paymentId, process, externalId);

    }
}

