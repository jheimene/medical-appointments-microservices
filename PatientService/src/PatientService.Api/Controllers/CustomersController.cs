using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientService.Application.Customers.Queries.GetCustomerAddressById;
using PatientService.Application.Customers.Commands.CreateCustomer;
using PatientService.Application.Customers.Dtos;
using PatientService.Application.Customers.Queries.GetByIdCustomer;
using PatientService.Application.Customers.Commands.AddCustomerAddress;
using PatientService.Api.Common;

namespace PatientService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.Match(
                patientId => CreatedAtAction(nameof(GetById), new { customerId = patientId }, new { patientId }),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpGet("{customerId:guid}", Name = "Patient_GetById")]
        public async Task<IActionResult> GetById(Guid customerId)
        {
            var result = await _mediator.Send(new GetByIdCustomerQuery(customerId));
            await Task.Delay(6);
            return result.Match(
                patient => Ok(patient),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpPost("{customerId:guid}/addresses")]
        public async Task<IActionResult> AddAddress(Guid customerId, [FromBody] AddCustomerAddressRequestDto request)
        {
            var result = await _mediator.Send(new AddCustomerAddressCommand(
                customerId,
                request.Street,
                request.District,
                request.Province,
                request.Departament,
                request.Reference!,
                request.Label,
                request.IsDefault
            ));
            return CreatedAtAction(nameof(GetPatientAddress), new { addressId = result }, new { result });
        }

        [HttpGet("{customerId:guid}/addresses/{addressId:guid}")]
        public async Task<IActionResult> GetPatientAddress(Guid customerId, Guid addressId)
        {
            var result = await _mediator.Send(new GetCustomerAddressByIdQuery(customerId, addressId));
            return Ok(result);
        }

        [HttpGet("{customerId:guid}/addresses")]
        public async Task<IActionResult> GetPatientAddressAll(Guid customerId)
        {
            var result = await _mediator.Send(new GetByIdCustomerQuery(customerId));
            return Ok();
        }
    }
}