using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Orders.Saga.Configuration
{
    public static class MassTransitSagaConfiguration
    {

        public static IServiceCollection AddMassTransitWithSaga(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(m =>
            {
                // Configurar Saga y State Machine aquí
                m.AddSagaStateMachine<OrderStateMachine, OrderState>()
                    .InMemoryRepository(); // O usar otro tipo de repositorio según sea necesario

                m.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqOptions = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();
                    cfg.Host(rabbitMqOptions!.Host, (ushort)rabbitMqOptions.Port, "/", h =>
                    {
                        h.Username(rabbitMqOptions.UserName);
                        h.Password(rabbitMqOptions.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
