using MassTransit;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderService(
            IOrderRepository orderRepository,
            IPublishEndpoint publishEndpoint
        )
        {
            this._orderRepository = orderRepository;
            this._publishEndpoint = publishEndpoint;
        }

        public Task CancelOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task ConfirmOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateOrderAsync(Guid customerId, List<(Guid productId, int quantity)> items)
        {
            // Validar que exista el evento y las zonas esten disponibles
            
            var order = Order.Create(customerId, new List<OrderDetail>());

            await _orderRepository.CreateAsync(order);

            // Lanzamos el evento para realizar la reserva
            //var orderCreated = new OrderCreatedEvent(order.Id, customerId, Guid.NewGuid(), null, new List<ZoneRequest>());


            // Publicando el mensaje con Mass Transict
            //await _publishEndpoint.Publish(orderCreated);

            return order.OrderId;
        }
    }
}
