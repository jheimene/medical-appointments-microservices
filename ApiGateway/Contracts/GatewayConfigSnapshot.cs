
namespace Security.ApiGateway.Yarp.Contracts
{
    public sealed record GatewayConfigSnapshot(
        IReadOnlyList<GatewayRouteDefinition> Routes,
        IReadOnlyList<GatewayClusterDefinition> Clusters
    );
}
