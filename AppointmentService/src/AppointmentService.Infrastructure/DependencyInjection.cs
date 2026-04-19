
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Persistence.Repositories;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using OrderService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Persistence.Factories;
using OrderService.Infrastructure.Persistence.Interceptors;
using OrderService.Application.Abstractions.Interfaces;
using OrderService.Application.Abstractions;
using OrderService.Application.Abstractions.Services;
using OrderService.Infrastructure.Services;
using Polly;
using Polly.Extensions.Http;
using OrderService.Infrastructure.Configuration;
using OrderService.Application.Abstractions.Providers;
using Polly.Timeout;
using Polly.Retry;
using Polly.CircuitBreaker;
using StackExchange.Redis;
using OrderService.Infrastructure.Idempotency;
using OrderService.Infrastructure.Configuration.Secrets;
using OrderService.Infrastructure.Persistence.Mongo;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSecretProviderConfiguration(configuration);
            services.AddPersistence(configuration);
            services.AddResiliencePolicies(configuration);

            services.AddScoped<IOrderNumberGenerator, SequentialOrderNumberGenerator>();
            services.Configure<IdempotencyOptions>(configuration.GetSection("Idempotency"));

            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var connectionsString = configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis connectionString no esta configurado.");
                return ConnectionMultiplexer.Connect(connectionsString);
            });

            services.AddScoped<IOrderIdempotencyService, RedisOrderIdempotencyService>();

            return services;
        }

        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var repositoryType = configuration.GetValue<RepositoryType>("RepositoryType");
            services.Configure<MongoSettings>(configuration.GetSection("MongoDB"));

            // 1. MongoClient y Database
            services.AddSingleton<MetricsCommandInterceptor>();
            services.AddSingleton<MongoCommandMetrics>();

            services.AddSingleton<IMongoClient>(sp =>
            {
                var secrets = sp.GetRequiredService<ISecretProvider>();
                var connectionString = secrets.GetSecretAsync("ConnectionStrings__OrderMongo").GetAwaiter().GetResult();

                //var connectionString = configuration.GetConnectionString("OrderMongo");
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                sp.GetRequiredService<MongoCommandMetrics>()
                    .Configure(settings);
                return new MongoClient(settings);
            });

            services.AddSingleton(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var mongoSettings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return mongoClient.GetDatabase(mongoSettings.DatabaseName); // Return IMongoDatabase
            });

            // 2. DbContext EF Core con MongoDB
            services.AddDbContext<OrderDbContext>((sp, options) =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .AddInterceptors(sp.GetRequiredService<MetricsCommandInterceptor>());
            }, ServiceLifetime.Singleton);


            services.AddSingleton<MongoDbContext>();

            services.AddSingleton<IOrderRepositoryProvider, OrderRepositoryProvider>();

            services.AddScoped(sp =>
            {
                return sp.GetRequiredService<IOrderRepositoryProvider>().GetOrderRepository(repositoryType);
            }); // Return IOrderRepository por defecto usando EF Core. Puedes cambiar el tipo según tus necesidades.

            //services.AddScoped<IOrderRepository, OrderRepositoryMongo>();
            //services.AddScoped<IOrderRepository, OrderRepositoryEF>(); 

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddResiliencePolicies(this IServiceCollection services, IConfiguration configuration)
        {
            //var timeoutPolicy = GetTimeoutPolicy();
            //var retryPolicy = GetRetryPolicy();
            //var circuitBreakerPolicy = GetCircuitBreakerPolicy();
            //services.AddSingleton(timeoutPolicy);
            //services.AddSingleton(retryPolicy);
            //services.AddSingleton(circuitBreakerPolicy);

            var timeoutPolicy = GetTimeoutPolicy();
            var retryPolicy = GetRetryPolicy();
            var circuitBreakerPolicy = GetCircuitBreakerPolicy();
            services.AddHttpClient<ICustomerReadService, CustomerReadService>(client =>
            {
                var url = configuration.GetSection("Services:CustomerService").Value;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException("Falta la configuración 'Services:CustomerService'.");
                client.BaseAddress = new Uri(url);

            })
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(circuitBreakerPolicy)
                .AddPolicyHandler(timeoutPolicy);

            services.AddHttpClient<IProductReadService, ProductReadService>(client =>
            {
                var url = configuration.GetSection("Services:ProductService").Value;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException("Falta la configuración 'Services:ProductService'.");
                client.BaseAddress = new Uri(url);
                client.Timeout = TimeSpan.FromSeconds(3);
            })
                .AddPolicyHandler(retryPolicy);

            services.AddHttpClient<IPaymentProcessService, PaymentProcessService>(client =>
            {
                var url = configuration.GetSection("Services:PaymentService").Value;
                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException("Falta la configuración 'Services:PaymentService'.");
                client.BaseAddress = new Uri(url);
                client.Timeout = TimeSpan.FromSeconds(3);
            })
                .AddPolicyHandler(retryPolicy);
        }


        private static AsyncTimeoutPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(
                seconds: 5,
                timeoutStrategy: TimeoutStrategy.Optimistic, 
                (context, timespan, task) =>
                {
                    Console.WriteLine($"Timeout alcanzado despues de {timespan.TotalSeconds}");
                    return Task.CompletedTask;
                }
            ); 
        }

        private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)),
                    onRetry: (outcome, timespan, retryAttemp, context) =>
                    {
                        Console.WriteLine($"Reintento {retryAttemp} despues de {timespan.TotalSeconds} segundos {outcome.Exception?.Message}");
                    }
                );
        }

        private static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3, 
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan) =>
                    {
                        Console.WriteLine($"Circuito abierto por {timespan.TotalSeconds} debido a {outcome.Exception?.Message}");
                    },
                    onHalfOpen: () =>
                    {
                        Console.WriteLine($"Circuito semi abierto");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine($"Circuito cerrado - Esta trabajando con normalidad");
                    }
                );
        }

    }
}
