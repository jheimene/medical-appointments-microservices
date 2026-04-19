using Microsoft.IdentityModel.Tokens;
using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Common;
using Security.ApiGateway.Yarp.Config;
using Security.ApiGateway.Yarp.Configuration;
using Security.ApiGateway.Yarp.Contracts;
using Security.ApiGateway.Yarp.Data;
using Security.ApiGateway.Yarp.Models;
using Security.ApiGateway.Yarp.Services;
using System.Threading.RateLimiting;
using Yarp.ReverseProxy.Configuration;

namespace Segurity.ApiGateway
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddControllers();
            services.AddOpenApi();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API Gateway",
                    Version = "v1",
                    Description = "Microservicio de API Gateway con YARP"
                });
            });

            return services;
        }

        public static IServiceCollection AddApiGatewayYarp(this IServiceCollection services, IConfiguration configuration)
        {
            // Aquí puedes agregar servicios relacionados con YARP si es necesario
            // Por ejemplo, podrías registrar un proveedor de configuración personalizado para YARP

            //services.AddSingleton<YarpProvider>();
            //services.AddSingleton<DatabaseProxyConfigProvider>();
            //services.AddSingleton<IProxyConfigProvider>(sp =>
            //    sp.GetRequiredService<DatabaseProxyConfigProvider>());

            services.AddScoped<IGatewayConfigService, GatewayConfigService>();

            var mode = configuration.GetValue<GatewayConfigMode>("GatewayAdmin:Mode");           

            switch (mode)
            {
                case GatewayConfigMode.InMemory:
                    var initial = new GatewayConfigSnapshot(
                        InMemoryDataSeed.Routes.Select(r => new GatewayRouteDefinition(
                                RouteId: r.RouteId,
                                ClusterId: r.ClusterId!,
                                Order: r.Order,
                                Methods: [.. r.Match.Methods!.Select(m => (Method)Enum.Parse(typeof(Method), m))],
                                Path: r.Match.Path!,
                                RemovePrefix: "",
                                AuthorizationPolicy: r.AuthorizationPolicy
                            )).ToList(), 
                        InMemoryDataSeed.Clusters.Select(c => new GatewayClusterDefinition(
                                ClusterId: c.ClusterId!,
                                LoadBalancingPolicy: c.LoadBalancingPolicy,
                                HealthCheckPath: c.HealthCheck?.Active?.Path,
                                Destinations: c.Destinations!.Select(s => new GatewayDestinationDefinition(
                                    DestinationId: s.Key,
                                    Address: s.Value.Address
                                 )).ToList()
                            )).ToList()
                         );

                    services.AddSingleton(initial);
                    services.AddSingleton<IGatewayConfigStore, InMemoryGatewayConfigStore>();

                    services
                        .AddReverseProxy()
                        .LoadFromMemory(InMemoryDataSeed.Routes, InMemoryDataSeed.Clusters);

                    services.AddScoped<IGatewayRuntimeApplier, InMemoryGatewayRuntimeApplier>();
                    break;
                case GatewayConfigMode.AppSettings:
                    services
                        .AddReverseProxy()
                        .LoadFromConfig(configuration.GetSection("ReverseProxy"));

                    services.AddScoped<IGatewayConfigStore, FileGatewayConfigStore>();
                    services.AddScoped<IGatewayRuntimeApplier, FileGatewayRuntimeApplier>();
                    break;
                case GatewayConfigMode.Database:
                    services.AddSingleton<DatabaseProxyConfigProvider>();
                    services.AddSingleton<IProxyConfigProvider, DatabaseProxyConfigProvider>();

                    services
                        .AddReverseProxy();

                    services.AddScoped<IGatewayConfigStore, DbGatewayConfigStore>();
                    services.AddScoped<IGatewayRuntimeApplier, DbGatewayRuntimeApplier>();
                    break;
                default:
                    throw new InvalidOperationException("Modo de gateway no soportado.");
            }

            return services;
        }

        public static IServiceCollection AddAuthorizationAndPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            // Aquí puedes agregar servicios relacionados con la autorización si es necesario
            services.AddAuthentication("Bearer") // si luego lo integras con IdentityServer/Keycloak
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["IdentityServer:Authority"];  //"https://auth0"; // Entra ID
                    options.Audience = "yarp-gateway";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AccessUser", policy => policy.RequireAuthenticatedUser());


                // Clientes (Aplicaciones)
                options.AddPolicy("CanGetEvent", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "event.api.read");
                });

                // Usuarios 
                options.AddPolicy("CanGetEvent", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("role", "Finanzas");
                });
            });

            // Por ejemplo, podrías configurar políticas de autorización específicas para el API Gateway
            // Rate Limiting por IP o Throttings
            services.AddRateLimiter(_ => //_
            {
                _.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    // Clave de partición basada en la dirección IP
                    var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(key, _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromSeconds(10),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        });
                });

                _.AddPolicy<string>("ipPolicy", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromSeconds(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));

                _.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            return services;
        }

        public static WebApplication UseApiGateway(this WebApplication app)
        {
            // Aquí puedes configurar el middleware específico del API Gateway si es necesario
            // Por ejemplo, podrías agregar un endpoint para recargar la configuración de YARP
            app.MapPost("/reload", (YarpProvider provider) =>
            {
                provider.Reload();
                return Results.Ok("Rutas recargadas desde la base de datos");
            });
            return app;
        }

    }
}
