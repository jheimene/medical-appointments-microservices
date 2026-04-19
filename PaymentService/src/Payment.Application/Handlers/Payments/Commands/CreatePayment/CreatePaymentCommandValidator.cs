
using FluentValidation;

namespace PaymentService.Application.Handlers.Payments.Commands.CreatePayment;
public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El monto debe ser mayor a 0.");
    }


}
