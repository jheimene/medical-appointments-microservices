
using MediatR;
using ProductService.Domain.Products.Events;

namespace ProductService.Application.Products.DomainEventHandlers
{
    public sealed class ProductActivateDomainEventHandler : INotificationHandler<ProductActivateDomainEvent>
    {

        // Enviar un mensaje a RabbitMQ para notificar que el producto ha sido activado
        public Task Handle(ProductActivateDomainEvent notification, CancellationToken cancellationToken)
        {
            
            Console.WriteLine(notification.Name);

            return Task.CompletedTask;
        }
    }
}
