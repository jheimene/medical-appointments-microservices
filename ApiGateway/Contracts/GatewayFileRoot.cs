using System.Text.Json.Serialization;

namespace Security.ApiGateway.Yarp.Contracts
{

    public sealed class GatewayFileRoot
    {
        [JsonPropertyName("ReverseProxy")]
        public ReverseProxyFileSection ReverseProxy { get; set; } = new();

        public GatewayConfigSnapshot ToSnapshot()
        {
            var routes = ReverseProxy.Routes
                .Select(x => new GatewayRouteDefinition(
                    RouteId: x.Key,
                    ClusterId: x.Value.ClusterId,
                    Order: x.Value.Order,
                    Methods: x.Value.Methods,
                    Path: x.Value.Match.Path,
                    RemovePrefix: x.Value.Transforms?
                        .FirstOrDefault(t => t.ContainsKey("PathRemovePrefix"))?
                        .GetValueOrDefault("PathRemovePrefix"),
                    AuthorizationPolicy: x.Value.AuthorizationPolicy))
                .ToList();

            var clusters = ReverseProxy.Clusters
                .Select(x => new GatewayClusterDefinition(
                    ClusterId: x.Key,
                    LoadBalancingPolicy: x.Value.LoadBalancingPolicy,
                    HealthCheckPath: null,
                    Destinations: x.Value.Destinations
                        .Select(d => new GatewayDestinationDefinition(
                            DestinationId: d.Key,
                            Address: d.Value.Address))
                        .ToList()))
                .ToList();

            return new GatewayConfigSnapshot(routes, clusters);
        }

        public static GatewayFileRoot FromSnapshot(GatewayConfigSnapshot snapshot)
        {
            var root = new GatewayFileRoot();

            root.ReverseProxy.Routes = snapshot.Routes.ToDictionary(
                r => r.RouteId,
                r => new GatewayFileRoute
                {
                    ClusterId = r.ClusterId,
                    Order = r.Order,
                    AuthorizationPolicy = r.AuthorizationPolicy,
                    Match = new GatewayFileMatch
                    {
                        Path = r.Path
                    },
                    Transforms = string.IsNullOrWhiteSpace(r.RemovePrefix)
                        ? new List<Dictionary<string, string>>()
                        : new List<Dictionary<string, string>>
                        {
                        new()
                        {
                            ["PathRemovePrefix"] = r.RemovePrefix!
                        }
                        }
                },
                StringComparer.OrdinalIgnoreCase);

            root.ReverseProxy.Clusters = snapshot.Clusters.ToDictionary(
                c => c.ClusterId,
                c => new GatewayFileCluster
                {
                    LoadBalancingPolicy = c.LoadBalancingPolicy,
                    Destinations = c.Destinations.ToDictionary(
                        d => d.DestinationId,
                        d => new GatewayFileDestination
                        {
                            Address = d.Address
                        },
                        StringComparer.OrdinalIgnoreCase)
                },
                StringComparer.OrdinalIgnoreCase);

            return root;
        }
    }

    public sealed class ReverseProxyFileSection
    {
        public Dictionary<string, GatewayFileRoute> Routes { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, GatewayFileCluster> Clusters { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
