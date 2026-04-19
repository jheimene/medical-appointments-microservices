using BffService.DTOs;
using BffService.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BffService.Endpoints
{
    public static class OrderSummaryEndpoint
    {
        public static IEndpointRouteBuilder MapOrderSummaryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/orders")
                .WithTags("Orders");

            group.MapGet("/{orderId:guid}/summary", GetOrderSummaryByIdAsync)
                .Produces<OrderSummaryResponse>(StatusCodes.Status200OK)
                .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
                .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
                .WithName("GetOrderSummaryById")
                .WithDescription("Obtiene una orden detallada por su identificador.");

            return app;
        }



        public static async Task<Results<
            Ok<OrderSummaryResponse>,
            NotFound<ErrorResponse>,
            BadRequest<ErrorResponse>
            >>
            GetOrderSummaryByIdAsync([FromRoute] Guid orderId, IOrderSummaryComposer composer, CancellationToken cancellationToken)
        {
            if (orderId == Guid.Empty)
                return TypedResults.BadRequest(new ErrorResponse("invalid_order_id", "El orderId no es válido."));

            var result = await composer.ComposeAsync(orderId, cancellationToken);

            return result is null
                ? TypedResults.NotFound(new ErrorResponse("order_not_found", $"No se encontró la orden con id {orderId}."))
                : TypedResults.Ok(result);
        }
    }
}
