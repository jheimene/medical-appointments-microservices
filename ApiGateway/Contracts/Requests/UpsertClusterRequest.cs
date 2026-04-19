namespace Security.ApiGateway.Yarp.Contracts.Requests
{
    public sealed record UpsertClusterRequest(
        string ClusterId,
        string? LoadBalancingPolicy,
        string? HealthCheckPath,
        ICollection<UpsertDestinationRequest> Destinations
    );

    public sealed record UpsertDestinationRequest(
        string DestinationId,
        string Address
    );
}
