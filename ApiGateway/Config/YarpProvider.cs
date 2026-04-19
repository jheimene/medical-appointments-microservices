using Microsoft.EntityFrameworkCore;
using Security.ApiGateway.Yarp.Data;
using Yarp.ReverseProxy.Configuration;

namespace Security.ApiGateway.Yarp.Config
{
    public class YarpProvider(IServiceScopeFactory scopeFactory, InMemoryConfigProvider configProvider)
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly InMemoryConfigProvider _configProvider = configProvider;

        public void Reload()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<YarpConfigDbContext>();

            var dbClusters = db.Clusters.Include(c => c.Destinations).AsNoTracking().ToList();
            var dbRoutes = db.Routes.AsNoTracking().ToList();

            var clusters = dbClusters
                .Select(g => new ClusterConfig
                {
                    ClusterId = g.ClusterId,
                    LoadBalancingPolicy = string.IsNullOrEmpty(g.LoadBalancingPolicy) ? "Random" : g.LoadBalancingPolicy,
                    Destinations = g.Destinations.ToDictionary(k => k.DestinationId, v => new DestinationConfig() { Address = v.Address }),
                    HealthCheck = string.IsNullOrEmpty(g.HealthCheckPath) ? null : new() { Active = new() { Path = g.HealthCheckPath } }
                }).ToList();

            var routes = dbRoutes.Select(r => new RouteConfig
            {
                RouteId = r.RouteId,
                Order = r.Order,
                ClusterId = r.ClusterId,
                Match = new RouteMatch { Path = r.PathPattern },
                AuthorizationPolicy = r.AuthorizationPolicy,
                RateLimiterPolicy = r.RateLimitPolicy                
            }).ToList();

            _configProvider.Update(routes, clusters);
        }
    }
}
