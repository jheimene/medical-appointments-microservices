using Azure.Messaging.ServiceBus;
using Email.Worker.Abstractions;
using Email.Worker.Email;
using Email.Worker.Messaging;
using Email.Worker.Services;
using Email.Worker.Workers;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<EmailQueueSettings>(builder.Configuration.GetSection(EmailQueueSettings.SectionName));

builder.Services.Configure<EmailServiceSettings>(builder.Configuration.GetSection(EmailServiceSettings.SectionName));

builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<EmailQueueSettings>>().Value;
    return new ServiceBusClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddHttpClient<IEmailSender, ExternalEmailSender>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<EmailServiceSettings>>().Value;
    client.BaseAddress = new Uri(settings.BaseUrl);
});

builder.Services.AddHostedService<WorkerEmailQueueConsumer>();

var host = builder.Build();

await host.RunAsync();
