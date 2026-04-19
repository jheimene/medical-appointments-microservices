namespace Security.ApiGateway.Yarp.Contracts
{
    public sealed record GatewayClusterDefinition(
        string ClusterId,
        string? LoadBalancingPolicy,
        string? HealthCheckPath,
        IReadOnlyList<GatewayDestinationDefinition> Destinations
    );

    public sealed record GatewayDestinationDefinition(
        string DestinationId,
        string Address
    );
}
