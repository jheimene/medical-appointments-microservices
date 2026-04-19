namespace Security.ApiGateway.Yarp.Contracts.Responses
{
    public sealed record ClusterResponse(
        string ClusterId,
        string? LoadBalancingPolicy,
        string? HealthCheckPath,
        IReadOnlyDictionary<string, string> Destinations
    );
}
