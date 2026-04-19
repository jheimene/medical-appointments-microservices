using Security.ApiGateway.Yarp.Models;

namespace Security.ApiGateway.Yarp.Contracts
{
    public sealed class GatewayFileRoute
    {
        public string ClusterId { get; set; } = default!;
        public int? Order { get; set; }
        public string? AuthorizationPolicy { get; set; }
        public ICollection<Method> Methods { get; set; } = default!;
        public GatewayFileMatch Match { get; set; } = new();

        public List<Dictionary<string, string>>? Transforms { get; set; }
            = new();
    }

    public sealed class GatewayFileMatch
    {
        public string Path { get; set; } = default!;
    }
}
