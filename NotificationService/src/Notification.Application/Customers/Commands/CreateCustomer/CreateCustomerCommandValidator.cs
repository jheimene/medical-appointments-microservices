using FluentValidation;

namespace DispatchService.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido.")
                .MaximumLength(100).WithMessage("El apellido no puede exceder los 100 caracteres.");

            RuleFor(x => x.DocumentType)
                .NotEmpty().WithMessage("El tipo de documento es requerido")
                .MustAsync(IsNotValidDocumentType).WithMessage("El tipo de documento no es válido.");

            //RuleFor(x => x.DocumentNumber)
            //    .NotEmpty().WithMessage("El número de documento es requerido.")
            //    .MaximumLength(20).WithMessage("El número de documento no puede exceder los 50 caracteres.")
            //    .MustAsync(BeUniqueDocumentNumber).WithMessage("El número de documento ya existe.");
            ////.MustAsync(async (docNumber, cancellationToken) => !await BeUniqueDocumentNumber(docNumber, cancellationToken)).WithMessage("El número de documento ya existe.");

            RuleFor(x => x)
                .Must((x) => IsValidDocumentNumber(x.DocumentNumber, x.DocumentType)).WithMessage("El número de documento no es válido para el tipo de documento especificado.")
                .OverridePropertyName($"documentNumber");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El email no es válido.");

        }

        private async Task<bool> BeUniqueDocumentNumber(string documentNumber, CancellationToken cancellationToken)
        {
            return !await _customerRepository.ExistsByDocumentNumberAsync(documentNumber, cancellationToken);
        }

        private async Task<bool> IsNotValidDocumentType(string documentType, CancellationToken cancellationToken)
        {
            return EnumParsing.TryParseEnum<IdentityDocumentType>(documentType, out _);
        }

        private bool IsValidDocumentNumber(string documentNumber, string documentType)
        {
            if (!EnumParsing.TryParseEnum<IdentityDocumentType>(documentType, out var docType))
                return false;
            return DocumentRules.IsValid(docType, documentNumber.Trim());
        }

        private async Task<bool> IsValidBirthDate(DateOnly? birthDate, CancellationToken cancellationToken)
        {
            return !birthDate.HasValue || birthDate.Value <= DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
