namespace Security.ApiGateway.Yarp.Models
{
    public sealed class ProxyDestionation
    {
        public int Id { get; set; }
        public int ClusterId { get; set; }
        public string DestinationId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public ProxyCluster Cluster { get; set; } = default!;
    }
}
