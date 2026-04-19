using Microsoft.EntityFrameworkCore;
using Security.ApiGateway.Yarp.Data;
using Yarp.ReverseProxy.Configuration;

namespace Security.ApiGateway.Yarp.Config
{
    public class DatabaseProxyConfigProvider : IProxyConfigProvider
    {
        private DatabaseProxyConfig _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DatabaseProxyConfigProvider> _logger;

        public DatabaseProxyConfigProvider(
            IServiceScopeFactory scopeFactory,
            ILogger<DatabaseProxyConfigProvider> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _config = LoadConfig();
        }

        public IProxyConfig GetConfig() => _config;

        public async Task ReloadAsync()
        {
            _logger.LogInformation("Recargando configuración de YARP desde BD...");
            _config = LoadConfig();
            _config?.SignalChange();
            await Task.CompletedTask;
        }

        private DatabaseProxyConfig LoadConfig()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<YarpConfigDbContext>();

            var clustersDb = context.Clusters
                .Include(c => c.Destinations)
                .AsNoTracking()
                .Where(c => c.IsActive)
                .ToList();

            var routesDb = context.Routes
                .AsNoTracking()
                .Where (c => c.IsActive)
                //.Include(r => r.Cluster)
                //.Where(r => r.IsActive && r.Cluster.IsActive)
                .ToList();

            var clusters = clustersDb
                .Select(c =>
                {
                    var destinations = c.Destinations.ToDictionary(d => d.DestinationId, d => new DestinationConfig() { Address = d.Address });

                    return new ClusterConfig
                    {
                        ClusterId = c.ClusterId,
                        Destinations = destinations,
                        HealthCheck = string.IsNullOrEmpty(c.HealthCheckPath) ? null : new() { Active = new() { Path = c.HealthCheckPath }},
                        LoadBalancingPolicy = string.IsNullOrEmpty(c.LoadBalancingPolicy) ? "Random" : c.LoadBalancingPolicy
                    };
                })
                .ToList();

            var routes = routesDb
                .Select(r =>
                {
                    //var transforms = string.IsNullOrEmpty(r.TransformsJson)
                    //    ? new List<IDictionary<string, string>>()
                    //    : System.Text.Json.JsonSerializer
                    //        .Deserialize<List<Dictionary<string, string>>>(r.TransformsJson)
                    //        ?? new();

                    //var transforms = string.IsNullOrEmpty(r.TransformsJson)
                    //? new List<Dictionary<string, string>>()   // 👈 mismo tipo que la deserialización
                    //: System.Text.Json.JsonSerializer
                    //    .Deserialize<List<Dictionary<string, string>>>(r.TransformsJson)
                    //    ?? new List<Dictionary<string, string>>();


                    return new RouteConfig
                    {
                        RouteId = r.RouteId,
                        Order = r.Order,
                        Match = new()
                        {
                            //Hosts = string.IsNullOrEmpty(r.MatchHost)
                            //    ? null
                            //    : new[] { r.MatchHost },
                            Path = r.PathPattern
                        },
                        ClusterId = r.ClusterId,
                        //Transforms = transforms,
                        AuthorizationPolicy = r.AuthorizationPolicy,
                        RateLimiterPolicy = r.RateLimitPolicy
                    };
                })
                .ToList();

            _logger.LogInformation("Configuración cargada: {RouteCount} rutas, {ClusterCount} clusters",
                routes.Count, clusters.Count);

            return new DatabaseProxyConfig(routes, clusters, Guid.NewGuid().ToString("N"));
        }

        public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            var newConfig = new DatabaseProxyConfig(routes, clusters, Guid.NewGuid().ToString("N"));
            var oldConfig = Interlocked.Exchange(ref _config, newConfig);
            oldConfig?.SignalChange();
        }
    }
}
