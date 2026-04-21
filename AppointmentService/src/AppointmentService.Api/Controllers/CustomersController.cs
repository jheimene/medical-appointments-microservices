using MediatR;
using Microsoft.AspNetCore.Mvc;
using AppointmentService.Application.Customers.Commands.CreateCustomer;
using AppointmentService.Application.Customers.Queries.GetByIdCustomer;
using AppointmentService.Api.Common;

namespace AppointmentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.Match(
                appointmentId => CreatedAtAction(nameof(GetById), new { appointmentId }, new { appointmentId }),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpGet("{appointmentId:guid}", Name = "Appointment_GetById")]
        public async Task<IActionResult> GetById(Guid appointmentId)
        {
            var result = await _mediator.Send(new GetByIdCustomerQuery(appointmentId));
            await Task.Delay(6);
            return result.Match(
                appointment => Ok(appointment),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }
    }
}