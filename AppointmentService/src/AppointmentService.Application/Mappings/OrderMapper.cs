
using OrderService.Application.Dtos;
using OrderService.Application.UseCases.Orders.CreateOrder;
using OrderService.Domain.Orders;

namespace OrderService.Application.Mappings
{
    public static class OrderMapper
    {
        public static CreateOrderCommandResponse ToCreateOrderReponse(this Order order)
        {
            return new CreateOrderCommandResponse(
                order.OrderId,
                order.OrderNumber,
                order.Status.ToString(),
                order.Currency,
                order.Subtotal,
                order.DiscountTotal,
                order.TaxTotal,
                order.Total
            );
        }

        public static OrderDetailDto ToOrderDetailResponse(this Order order) {
            return new OrderDetailDto(
                order.OrderId,
                order.OrderNumber,
                order.Status.ToString(),
                order.Currency,
                order.Subtotal,
                order.DiscountTotal,
                order.TaxTotal,
                order.Total
            );
        }
    }
}
