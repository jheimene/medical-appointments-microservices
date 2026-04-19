
using OrderService.Infrastructure.Messaging;

namespace OrderService.Api.BackgroundServices
{
    public class RabbitMqConsumersBackgroundService : BackgroundService
    {
        private readonly string exchange = "event.api.fanout";
        private readonly EventPublishedRabbitMqConsumer _consumer;

        public RabbitMqConsumersBackgroundService(EventPublishedRabbitMqConsumer consumer)
        {
            this._consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Inicializado el Background Service del Consumers Rabbit Mq");
            await _consumer.StartAsync(exchange, "order.api.queue");
        }
    }
}
