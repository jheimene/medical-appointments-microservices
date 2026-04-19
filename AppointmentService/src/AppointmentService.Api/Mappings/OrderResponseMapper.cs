using OrderService.Api.Constracts.Responses;
using OrderService.Application.Dtos;
using OrderService.Application.UseCases.Orders.CreateOrder;

namespace OrderService.Api.Mappings
{
    public static class OrderResponseMapper
    {
        public static OrderDetailResponse ToOrderDetailReponse(this OrderDetailDto orderDto)
        {
            return new OrderDetailResponse(
                orderDto.OrderId,
                orderDto.OrderNumber,
                orderDto.Status.ToString(),
                orderDto.Currency,
                orderDto.Subtotal,
                orderDto.DiscountTotal,
                orderDto.TaxTotal,
                orderDto.Total,
                null
            );
        }

        public static CreateOrderResponse ToCreateOrderReponse(this CreateOrderCommandResponse createOrderCommand)
        {
            return new CreateOrderResponse(
                createOrderCommand.OrderId,
                createOrderCommand.OrderNumber,
                createOrderCommand.Status.ToString()
            );
        }
    }
}
