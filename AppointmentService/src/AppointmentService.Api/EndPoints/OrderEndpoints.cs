using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Constracts.Requests;
using OrderService.Api.Constracts.Responses;
using OrderService.Api.Mappings;
using OrderService.Application.UseCases.Orders.CreateOrder;
using OrderService.Application.UseCases.Orders.GetByIdOrder;

namespace OrderService.Api.EndPoints
{
    public static class OrderEndpoints
    {

        public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/orders")
                .WithTags("Orders");

            group.MapPost("/", CreateOrderAsync)
                .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
                .Produces<ErrorResponse>(StatusCodes.Status201Created)
                .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
                .WithName("CreateOrder")
                .WithSummary("Crea una nueva orden")
                .WithDescription("Crea una orden aplicando control de idempotencia mediante el header Idempotency-Key.");

            group.MapGet("/{orderId:guid}", GetOrderByIdAsync)
                .Produces<OrderDetailResponse>(StatusCodes.Status200OK)
                .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
                .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
                .WithName("GetOrderById")
                .WithSummary("Crea una nueva orden")
                .WithDescription("Obtiene una orden por su identificador.");

            return app;
        }


        public static async Task<
            Results<
                Created<CreateOrderResponse>,
                BadRequest<ErrorResponse>,
                Conflict<ErrorResponse>>>
        CreateOrderAsync(
            [FromBody] CreateOrderRequest request,
            [FromHeader(Name = "Idempotency-Key")] string idEmpotencyKey,
            [FromServices] CreateOrderCommandHandler handler, 
            CancellationToken cancellationToken
        )
        {
            //if (!Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyHeader) ||
            //string.IsNullOrWhiteSpace(idempotencyHeader))
            //{
            //    return Results.BadRequest(new { message = "Falta el header Idempotency-Key." });
            //}
            if (request.Items is null || request.Items.Count == 0)
            {
                return TypedResults.BadRequest(new ErrorResponse("items_required", "La orden debe contener al menos un items."));
            }

            var command = new CreateOrderCommand(
                CustomerId: request.CustomerId,
                Currency: request.Currency,
                CreatedBy: request.CreatedBy,
                Notes: request.Notes,
                Provider: request.Provider,
                IdempotencyKey: idEmpotencyKey,
                Items: [.. request.Items.Select(i => new CreateOrderItemCommand(i.ProductId, i.Quantity, i.Price))]
             );

            var result  = await handler.Handle(command, cancellationToken);

            if (!result.IsSuccess) {
                //return TypedResults.Conflict(new ErrorResponse("order_creation_error", $"No se pudo crear la orden "));
                return TypedResults.Conflict(new ErrorResponse("order_creation_error", $"{result.Error!.Message}"));
            }

            return TypedResults.Created($"/api/orders/{result.Value!.OrderId}", result.Value.ToCreateOrderReponse());
        }


        public static async Task<Results<
            Ok<OrderDetailResponse>,
            NotFound<ErrorResponse>,
            BadRequest<ErrorResponse>
            >>
        GetOrderByIdAsync(
            [FromRoute] Guid orderId, 
            [FromServices] GetByIdOrderQueryHandler handler, 
            CancellationToken cancellationToken
        ) {
            if (orderId == Guid.Empty)
                return TypedResults.BadRequest(new ErrorResponse("invalid_order_id", "El orderId no es válido."));

            var result = await handler.Handle(new GetByIdOrderQuery(orderId), cancellationToken);

            if (!result.IsSuccess || result.Value is null)
                return TypedResults.NotFound(new ErrorResponse("order_not_found", "No se encontro la orden.")); 


            return TypedResults.Ok(result.Value.ToOrderDetailReponse());
        }


    }
}
