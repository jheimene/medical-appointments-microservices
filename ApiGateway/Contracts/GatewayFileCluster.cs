namespace Security.ApiGateway.Yarp.Contracts
{
    public sealed class GatewayFileCluster
    {
        public string? LoadBalancingPolicy { get; set; }

        public Dictionary<string, GatewayFileDestination> Destinations { get; set; }
            = new(StringComparer.OrdinalIgnoreCase);
    }

    public sealed class GatewayFileDestination
    {
        public string Address { get; set; } = default!;
    }
}
