
namespace Security.ApiGateway.Yarp.Models
{
    public sealed class ProxyCluster
    {
        public Guid Id { get; set; }
        public string ClusterId { get; set; } = default!;
        public string? LoadBalancingPolicy { get; set; }
        public string? HealthCheckPath { get; set; }
        //public string DestinationsJson { get; set; } = default!; // { "dest1": { "Address": "https://..." }, ... }
        public bool IsActive { get; set; } = true;

        public ICollection<ProxyDestionation> Destinations { get; set; } = [];

        //public ICollection<RouteConfig2> Routes { get; set; } = new List<RouteConfig2>();
    }
}
