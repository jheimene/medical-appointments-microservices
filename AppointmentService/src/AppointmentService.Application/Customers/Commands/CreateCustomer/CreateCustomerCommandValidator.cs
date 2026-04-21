using FluentValidation;

namespace AppointmentService.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("El ID del paciente es requerido.");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("El ID del médico es requerido.");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage("La fecha de la cita es requerida.")
                .GreaterThan(DateTime.Now).WithMessage("La fecha de la cita debe ser futura.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("El motivo de la cita es requerido.")
                .MaximumLength(255).WithMessage("El motivo no puede exceder los 255 caracteres.");
        }
    }
}