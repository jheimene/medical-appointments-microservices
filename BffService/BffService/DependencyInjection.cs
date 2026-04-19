using BffService.Clients;
using BffService.Interfaces;
using BffService.Services;
using Microsoft.OpenApi;
using System.Net;

namespace BffService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BffService API",
                    Description = "An ASP.NET Core Web API for BffService",
                    Contact = new OpenApiContact
                    {
                        Name = "Ronald Terrones",
                        Email = "ronald.tc14@gmail.com",
                        Url = new Uri("https://example.com/contact"),
                    },
                });
            }
            );
            services.AddHealthChecks();

            services.AddHttpClient<IOrderServiceClient, OrderServiceClient>(client =>
            {
                var url = configuration.GetSection("Services:OrderService").Value;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException("Falta la configuración 'Services:OrderService'.");

                client.BaseAddress = new Uri(url);
                client.DefaultRequestVersion = HttpVersion.Version20;
                client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            });

            services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(client =>
            {
                var url = configuration.GetSection("Services:CustomerService").Value;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException("Falta la configuración 'Services:CustomerService'.");

                client.BaseAddress = new Uri(url);
                client.DefaultRequestVersion = HttpVersion.Version20;
                client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            });

            services.AddHttpClient<IPaymentServiceClient, PaymentServiceClient>(client =>
            {
                var url = configuration.GetSection("Services:PaymentService").Value;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException("Falta la configuración 'Services:PaymentService'.");

                client.BaseAddress = new Uri(url);
                client.DefaultRequestVersion = HttpVersion.Version20;
                client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            });

            services.AddHttpClient<IDispatchServiceClient, DispatchServiceClient>(client =>
            {
                var url = configuration.GetSection("Services:DispatchService").Value;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException("Falta la configuración 'Services:DispatchService'.");

                client.BaseAddress = new Uri(url);
                client.DefaultRequestVersion = HttpVersion.Version20;
                client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            });

            services.AddScoped<IOrderSummaryComposer, OrderSummaryComposer>();


            // AddGrpcReadyHttpClient<IOrderServiceClient, OrderServiceClient>(services, configuration, "OrderService");
            // AddGrpcReadyHttpClient<ICustomerServiceClient, CustomerServiceClient>(services, configuration, "CustomerService");
            // AddGrpcReadyHttpClient<IPaymentServiceClient, PaymentServiceClient>(services, configuration, "PaymentService");
            // AddGrpcReadyHttpClient<IDispatchServiceClient, DispatchServiceClient>(services, configuration, "DispatchService");


            return services;
        }

        private static void AddGrpcReadyHttpClient<TClient, TImplementation>(
            IServiceCollection services,
            IConfiguration configuration,
            string serviceName)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddHttpClient<TClient, TImplementation>(client =>
            {
                var url = configuration.GetSection($"Services:{serviceName}").Value;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException($"Falta la configuración 'Services:{serviceName}'.");

                client.BaseAddress = new Uri(url);
                client.DefaultRequestVersion = HttpVersion.Version20;
                client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            });
        }
    }
}
