using MediatR;
using ProductService.Domain.Products.Events;

namespace ProductService.Application.Products.DomainEventHandlers
{
    public class ProductSearchCreatedDomainEvent : INotificationHandler<ProductCreatedDomainEvent>
    {
        public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"ProductSearch created: {notification.Name} (ID: {notification.ProductId})");

            return Task.CompletedTask;  
        }
    }
}
