
using FluentValidation;

namespace OrderService.Application.UseCases.Orders.CreateOrder
{
    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator() {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("El cliente es obligatorio.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("No existen items.");
        }

    }
}
