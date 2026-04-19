using MediatR;
using PaymentService.Application.Handlers.Payments.Commands.CreatePayment;
using PaymentService.Application.Handlers.Payments.Queries.GetByEventIdPayment;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;



namespace PaymentService.Api.EndPoints
{
    public static class PaymentEndPoints
    {
        public static IEndpointRouteBuilder MapPaymentEndPoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/payments")
                    .WithTags("Payments");


            group.MapPost("/", async (CreatePaymentCommand command, IMediator mediator) =>
            {
                try
                {
                    var result = await mediator.Send(command);
                    return Results.Ok(new { paymentId = result.PaymentId, isSuccess = result.IsSuccess, transactionId = result.TransactionId });

                }
                catch (Exception)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
           .WithName("CreatePayment")
           .Produces<Guid>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/{orderId}", async (Guid orderId, IMediator mediator) =>
            {
                return await mediator.Send(new GetPaymentByOrderId(orderId));
            })
           .WithName("GetPaymentByOrderId")
           .Produces<Guid>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status500InternalServerError);

            return builder;
        }
    }
}
