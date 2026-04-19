using DispatchService.Application.Commmon.Interfaces;
using DispatchService.Application.Common.Helpers;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DispatchService.Application.Customers.Commands.CreateCustomer
{
    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Guid>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfwork, ILogger<CreateCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _unitOfWork = unitOfwork ?? throw new ArgumentNullException(nameof(unitOfwork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ErrorOr<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating customer with document number {DocumentNumber}", command.DocumentNumber);
            //_logger.LogInformation("Handler ejecutado a las {Timestamp}. HashCode: {HashCode}", DateTime.Now, GetHashCode());

            //// Validaciones (Con FluentValidation)
            //if (await _customerRepository.ExistsByDocumentNumberAsync(request.DocumentNumber, cancellationToken))
            //    throw new ArgumentException("El número de documento ya existe.");

            if (!EnumParsing.TryParseEnum<IdentityDocumentType>(command.DocumentType, out var documentType))
                return Error.Validation(code: "DocumentType.Invalid", description: $"DocumentType '{command.DocumentType}' no es válido.");

            var customer = Customer.Create(
                command.Name,
                command.LastName,
                documentType,
                command.DocumentNumber,
                "SISTEMAS",
                DateTime.Now, // Deberia venir de un servicio
                command.Email,
                command.PhoneNumber,
                command.BirthDate,
                command.Gender
             );
            
            _customerRepository.CreateAsync(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer.Id.Value;
        }
    }
}
