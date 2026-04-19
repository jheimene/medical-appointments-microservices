using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Security.ApiGateway.Yarp.Config
{
    public class DatabaseProxyConfig : IProxyConfig
    {
        private readonly CancellationTokenSource _cts = new();

        public DatabaseProxyConfig(
            IReadOnlyList<RouteConfig> routes, 
            IReadOnlyList<ClusterConfig> clusters,
            string revisionId)
        {
            Routes = routes;
            Clusters = clusters;
            RevisionId = revisionId;
            ChangeToken = new CancellationChangeToken(_cts.Token);
        }


        public string RevisionId { get; }
        public IReadOnlyList<RouteConfig> Routes { get; }
        public IReadOnlyList<ClusterConfig> Clusters { get; }
        public IChangeToken ChangeToken { get; }

        public void SignalChange() => _cts.Cancel();
    }
}
