using AppointmentService.Application.Commmon.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Customers.Commands.CreateCustomer
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
            _logger.LogInformation("Creating appointment for patient {PatientId} with doctor {DoctorId}", command.PatientId, command.DoctorId);

            var customer = Customer.Create(
                command.PatientId.ToString(),
                command.DoctorId.ToString(),
                IdentityDocumentType.DNI,
                command.AppointmentDate.ToString("yyyyMMdd"),
                "SISTEMAS",
                DateTime.Now,
                null,
                null,
                null,
                null
             );

            _customerRepository.CreateAsync(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer.Id.Value;
        }
    }
}