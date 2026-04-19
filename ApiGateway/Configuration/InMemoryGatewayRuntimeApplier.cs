using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Contracts;
using Yarp.ReverseProxy.Configuration;

namespace Security.ApiGateway.Yarp.Configuration
{
    public sealed class InMemoryGatewayRuntimeApplier(InMemoryConfigProvider provider) : IGatewayRuntimeApplier
    {
        private readonly InMemoryConfigProvider _provider =  provider;

        public Task ApplyAsync(GatewayConfigSnapshot snapshot, CancellationToken ct)
        {
            var routes = snapshot.Routes.Select(ToRouteConfig).ToArray();
            var clusters = snapshot.Clusters.Select(ToClusterConfig).ToArray();

            _provider.Update(routes, clusters);
            return Task.CompletedTask;
        }

        private static RouteConfig ToRouteConfig(GatewayRouteDefinition x)
        {
            var transforms = new List<IReadOnlyDictionary<string, string>>();

            if (!string.IsNullOrWhiteSpace(x.RemovePrefix))
            {
                transforms.Add(new Dictionary<string, string>
                {
                    ["PathRemovePrefix"] = x.RemovePrefix!
                });
            }

            return new RouteConfig
            {
                RouteId = x.RouteId,
                ClusterId = x.ClusterId,
                Order = x.Order,
                AuthorizationPolicy = x.AuthorizationPolicy,
                Match = new RouteMatch { Path = x.Path },
                Transforms = transforms
            };
        }

        private static ClusterConfig ToClusterConfig(GatewayClusterDefinition x)
        {
            return new ClusterConfig
            {
                ClusterId = x.ClusterId,
                LoadBalancingPolicy = x.LoadBalancingPolicy,
                Destinations = x.Destinations.ToDictionary(
                    d => d.DestinationId,
                    d => new DestinationConfig { Address = d.Address })
            };
        }
    }
}
