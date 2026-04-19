using BuildingBlocks.Application.Common.Errors;
using MediatR;

namespace OrderService.Application.UseCases.Orders.CreateOrder
{
    public sealed record CreateOrderCommand(
        Guid CustomerId,
        string Currency,
        string CreatedBy,
        string? Notes,
        //string? CorrelationId,
        string? Provider,
        string? IdempotencyKey,
        IReadOnlyCollection<CreateOrderItemCommand> Items
    ) : IRequest<Result<CreateOrderCommandResponse>>;


    public sealed record CreateOrderItemCommand(
        Guid ProductId,
        int Quantity,
        decimal Price
    );
}
