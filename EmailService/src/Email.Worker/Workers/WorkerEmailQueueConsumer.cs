using Azure.Messaging.ServiceBus;
using Email.Worker.Abstractions;
using Email.Worker.Messaging;
using Email.Worker.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Email.Worker.Workers
{
    public sealed class WorkerEmailQueueConsumer : BackgroundService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly IEmailService _emailService;
        private readonly ILogger<WorkerEmailQueueConsumer> _logger;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public WorkerEmailQueueConsumer(
            ServiceBusClient client,
            IOptions<EmailQueueSettings> options,
            IEmailService emailService,
            ILogger<WorkerEmailQueueConsumer> logger)
        {
            _emailService = emailService;
            _logger = logger;

            var settings = options.Value;

            _processor = client.CreateProcessor(settings.QueueName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 5,
                AutoCompleteMessages = false
            });

            _processor.ProcessMessageAsync += OnMessageReceivedAsync;
            _processor.ProcessErrorAsync += OnErrorAsync;
        }

        private async Task OnMessageReceivedAsync(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();
                _logger.LogInformation("Mensaje recibido de Service Bus: {Body}", body);

                var command = JsonSerializer.Deserialize<UserCreatedEvent>(body, _jsonOptions);
                if (command is null)
                {
                    _logger.LogWarning("No se pudo deserializar el mensaje a SendEmailCommand");
                    await args.DeadLetterMessageAsync(args.Message,
                        "DeserializationFailed",
                        "No se pudo deserializar SendEmailCommand");
                    return;
                }

                // Procesar el comando en la capa de aplicación
                await _emailService.ProcessEmailAsync(command);       

                await args.CompleteMessageAsync(args.Message);

                _logger.LogInformation("Mensaje procesado y completado. MessageId: {MessageId}",
                    args.Message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando mensaje. MessageId: {MessageId}", args.Message.MessageId);
                // Al no llamar CompleteMessageAsync, el mensaje será reintentado.
                // Si supera MaxDeliveryCount, irá a la DLQ.
            }
        }

        private Task OnErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception,
            "Error en ServiceBusProcessor. Fuente: {ErrorSource}, Entidad: {EntityPath}, Namespace: {Namespace}",
            args.ErrorSource, args.EntityPath, args.FullyQualifiedNamespace);
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando EmailQueueConsumer...");
            await _processor.StartProcessingAsync(stoppingToken);

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Normal cuando se apaga el servicio
            }

            _logger.LogInformation("Deteniendo EmailQueueConsumer...");
            await _processor.StopProcessingAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _processor.ProcessMessageAsync -= OnMessageReceivedAsync;
            _processor.ProcessErrorAsync -= OnErrorAsync;
            _processor.DisposeAsync().AsTask().GetAwaiter().GetResult();
            base.Dispose();
        }
    }
}
