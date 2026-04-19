using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Config;
using Security.ApiGateway.Yarp.Contracts;
using Yarp.ReverseProxy.Configuration;

namespace Security.ApiGateway.Yarp.Configuration
{
    public sealed class DbGatewayRuntimeApplier : IGatewayRuntimeApplier
    {
        private readonly DatabaseProxyConfigProvider _provider;

        public DbGatewayRuntimeApplier(
            DatabaseProxyConfigProvider provider
        )
        {
            _provider = provider;
        }
      
        public Task ApplyAsync(GatewayConfigSnapshot snapshot, CancellationToken cancellationToken)
        {
            var routes = snapshot.Routes.Select(r => new RouteConfig()
            {
                RouteId = r.RouteId,
                ClusterId = r.ClusterId,
                Order = r.Order,
                Match = new RouteMatch()
                {
                    Path = r.Path,
                    Methods = [.. r.Methods.Select(m => m.ToString())]      
                },                
                AuthorizationPolicy = r.AuthorizationPolicy,
            }).ToArray();
            var clusters = snapshot.Clusters.Select(c => new ClusterConfig()
            {
                ClusterId = c.ClusterId,
                LoadBalancingPolicy = c.LoadBalancingPolicy,
                Destinations = c.Destinations.ToDictionary(d => d.DestinationId, d => new DestinationConfig()
                {
                    Address = d.Address
                })
            }).ToArray();

            _provider.Update(routes, clusters);
            return Task.CompletedTask;
        }
    }
}
