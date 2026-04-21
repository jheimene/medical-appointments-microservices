using AppointmentService.Application.Abstractions.Clients;
using AppointmentService.Application.Commmon.Interfaces;
using AppointmentService.Domain.Entities;
using AppointmentService.Domain.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Customers.Commands.CreateCustomer
{
    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Guid>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;
        private readonly IPatientServiceClient _patientServiceClient;
        private readonly IDoctorServiceClient _doctorServiceClient;

        public CreateCustomerCommandHandler(
            IAppointmentRepository appointmentRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateCustomerCommandHandler> logger,
            IPatientServiceClient patientServiceClient,
            IDoctorServiceClient doctorServiceClient)
        {
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _patientServiceClient = patientServiceClient;
            _doctorServiceClient = doctorServiceClient;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating appointment for patient {PatientId} with doctor {DoctorId}", command.PatientId, command.DoctorId);

            var patientExists = await _patientServiceClient.PatientExistsAsync(command.PatientId);
            if (!patientExists)
                return Error.NotFound("Patient.NotFound", $"Patient with id {command.PatientId} not found.");

            var doctorExists = await _doctorServiceClient.DoctorExistsAsync(command.DoctorId);
            if (!doctorExists)
                return Error.NotFound("Doctor.NotFound", $"Doctor with id {command.DoctorId} not found.");

            var appointment = Appointment.Create(
                command.PatientId,
                command.DoctorId,
                command.AppointmentDate,
                command.Reason
            );

            await _appointmentRepository.CreateAsync(appointment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return appointment.AppointmentId;
        }
    }
}