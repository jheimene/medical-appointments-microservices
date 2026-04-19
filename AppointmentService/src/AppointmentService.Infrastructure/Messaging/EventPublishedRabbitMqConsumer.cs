
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Dtos;
using OrderService.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderService.Infrastructure.Messaging
{
    public class EventPublishedRabbitMqConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IChannel _channel;

        public EventPublishedRabbitMqConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMq:Host"]!,
                Port = int.Parse(configuration["RabbitMq:Port"]!),
                UserName = configuration["RabbitMq:Username"]!,
                Password = configuration["RabbitMq:password"]!,
            };
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(string exchange, string queueName)
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = await _connectionFactory.CreateConnectionAsync();
            }

            _channel ??= await _connection.CreateChannelAsync();

            // Declaración del Exchange
            await _channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Fanout, durable: true, autoDelete: false);

            // Declaremos la cola
            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            // Asociando la cola con el Exchange
            await _channel.QueueBindAsync(queue: queueName, exchange: exchange, routingKey: "");

            Console.WriteLine("Rabbit inicializado");

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, eventA) =>
            {
                try
                {
                    var body = eventA.Body.ToArray();
                    var jsonStr = Encoding.UTF8.GetString(body);

                    var eventPublished = JsonSerializer.Deserialize<EventPublishedEvent>(jsonStr);

                    using var scope = _scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<IEventInventoryService>();

                    if (!string.IsNullOrEmpty(jsonStr))
                    {

                        await service.CreateAsync(eventPublished);
                        Console.WriteLine($"Mensaje Leido en la col {queueName}");

                        await _channel.BasicAckAsync(eventA.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al leer el mensaje {ex.Message}");
                }
            };

            await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer);

        }
    }
}
