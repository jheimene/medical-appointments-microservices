using MediatR;
using Microsoft.AspNetCore.Mvc;
using CustomerService.Application.Customers.Queries.GetCustomerAddressById;
using CustomerService.Application.Customers.Commands.CreateCustomer;
using CustomerService.Application.Customers.Dtos;
using CustomerService.Application.Customers.Queries.GetByIdCustomer;
using CustomerService.Application.Customers.Commands.AddCustomerAddress;
using CustomerService.Api.Common;

namespace CustomerService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            //return CreatedAtAction(nameof(Get), new { customerId = result }, new { result });

            return result.Match(
                customerId => CreatedAtAction(nameof(GetById), new { customerId }, new { customerId }),
                //customerId => CreatedAtRoute("Customer_GetById", new { customerId }, new { customerId }),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpGet("{customerId:guid}", Name = "Customer_GetById")]
        public async Task<IActionResult> GetById(Guid customerId)
        {
            var result = await _mediator.Send(new GetByIdCustomerQuery(customerId));

            await Task.Delay(6);
            //return result is null ? NotFound() : Ok(result);
            return result.Match(
                customer => Ok(customer),
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

            return CreatedAtAction(nameof(GetCustomerAddress), new { customerAddressId = result }, new { result} );
        }

        [HttpGet("{customerId:guid}/addresses/{addressId:guid}")]
        public async Task<IActionResult> GetCustomerAddress(Guid customerId, Guid addressId)
        {
            var result = await _mediator.Send(new GetCustomerAddressByIdQuery(customerId, addressId));
            return Ok(result);
        }

        [HttpGet("{customerId:guid}/addresses")]
        public async Task<IActionResult> GetCustomerAddressAll(Guid customerId)
        {
            var result = await _mediator.Send(new GetByIdCustomerQuery(customerId));
            return Ok();
        }

    }
}
