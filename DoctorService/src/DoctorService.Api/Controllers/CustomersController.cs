using MediatR;
using Microsoft.AspNetCore.Mvc;
using DoctorService.Application.Customers.Queries.GetCustomerAddressById;
using DoctorService.Application.Customers.Commands.CreateCustomer;
using DoctorService.Application.Customers.Dtos;
using DoctorService.Application.Customers.Queries.GetByIdCustomer;
using DoctorService.Application.Customers.Commands.AddCustomerAddress;
using DoctorService.Api.Common;

namespace DoctorService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.Match(
                doctorId => CreatedAtAction(nameof(GetById), new { doctorId }, new { doctorId }),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpGet("{doctorId:guid}", Name = "Doctor_GetById")]
        public async Task<IActionResult> GetById(Guid doctorId)
        {
            var result = await _mediator.Send(new GetByIdCustomerQuery(doctorId));
            await Task.Delay(6);
            return result.Match(
                doctor => Ok(doctor),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpPost("{doctorId:guid}/addresses")]
        public async Task<IActionResult> AddAddress(Guid doctorId, [FromBody] AddCustomerAddressRequestDto request)
        {
            var result = await _mediator.Send(new AddCustomerAddressCommand(
                doctorId,
                request.Street,
                request.District,
                request.Province,
                request.Departament,
                request.Reference!,
                request.Label,
                request.IsDefault
            ));
            return CreatedAtAction(nameof(GetDoctorAddress), new { addressId = result }, new { result });
        }

        [HttpGet("{doctorId:guid}/addresses/{addressId:guid}")]
        public async Task<IActionResult> GetDoctorAddress(Guid doctorId, Guid addressId)
        {
            var result = await _mediator.Send(new GetCustomerAddressByIdQuery(doctorId, addressId));
            return Ok(result);
        }

        [HttpGet("{doctorId:guid}/addresses")]
        public async Task<IActionResult> GetDoctorAddressAll(Guid doctorId)
        {
            var result = await _mediator.Send(new GetByIdCustomerQuery(doctorId));
            return Ok();
        }
    }
}