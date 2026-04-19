using FluentValidation;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Behaviors;
using OrderService.Application.UseCases.Orders.CreateOrder;
using OrderService.Application.UseCases.Orders.GetByIdOrder;

namespace OrderService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assembly);
                cfg.NotificationPublisher = new TaskWhenAllPublisher();
            });

            // FluentValidation: registra All (incluye internal)
            services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            services.AddScoped<CreateOrderCommandHandler>(); 
            services.AddScoped<GetByIdOrderQueryHandler>();

            //services.AddScoped<IOrderService, OrderService>();

            //services.AddScoped<ITicketReservationService, TicketReservationService>();

            return services;
        }
    }
}
