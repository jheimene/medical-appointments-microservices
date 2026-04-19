
namespace OrderService.Application.UseCases.Orders.CreateOrder
{
    public sealed record CreateOrderCommandResponse(
        Guid OrderId,
        string OrderNumber,
        string Status,
        string Currency,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal TaxtTotal,
        decimal Total
    );

}
