using Yarp.ReverseProxy.Configuration;

namespace Security.ApiGateway.Yarp.Data
{
    public static class InMemoryDataSeed
    {

        public static List<RouteConfig> Routes = new()
        {
            new RouteConfig
            {
                RouteId = "customerServiceRoute",
                ClusterId = "customerCluster",
                Match = new RouteMatch { Path = "/api/customers/{**catch-all}", Methods = ["GET", "POST", "PUT"] },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() {
                        { "PathRemovePrefix", "" }
                    }
                },
            },
            new RouteConfig
            {
                RouteId = "productServiceRoute",
                ClusterId = "productCluster",
                Match = new RouteMatch { Path = "/api/products/{**catch-all}", Methods = ["GET", "POST", "PUT"] },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() {
                        { "PathRemovePrefix", "" }
                    }
                },
            }
        };

        public static List<ClusterConfig> Clusters = new()
        {
            new ClusterConfig
            {
                ClusterId = "customerCluster",
                LoadBalancingPolicy = "RoundRobin",
                HealthCheck = new() { Active = new() { Path = "/health" } },
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "server1", new DestinationConfig { Address = "http://192.168.18.150:5500" } },
                    { "server2", new DestinationConfig { Address = "http://192.168.18.151:5500" } }
                }
            },
            new ClusterConfig
            {
                ClusterId = "productCluster",
                LoadBalancingPolicy = "RoundRobin",
                HealthCheck = new() { Active = new() { Path = "/health" } },
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "server1", new DestinationConfig { Address = "http://192.168.18.150:5600" } },
                    { "server2", new DestinationConfig { Address = "http://192.168.18.151:5600" } }
                }
            }
        };

        

    }
}
