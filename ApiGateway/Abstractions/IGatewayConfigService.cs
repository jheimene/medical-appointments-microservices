using Security.ApiGateway.Yarp.Contracts;
using Security.ApiGateway.Yarp.Contracts.Responses;

namespace Security.ApiGateway.Yarp.Abstractions
{
    public interface IGatewayConfigService
    {
        Task<RouteResponse> GetRoute(string routeId, CancellationToken cancellationToken); 
        Task<IReadOnlyList<RouteResponse>> GetRoutes(CancellationToken cancellationToken);
        Task CreateRouteAsync(GatewayRouteDefinition route, CancellationToken cancellationToken);
        Task UpdateRouteAsync(GatewayRouteDefinition route, CancellationToken cancellationToken);
        Task<ClusterResponse> GetCluster(string clusterId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ClusterResponse>> GetClusters(CancellationToken cancellationToken);
        Task CreateClusterAsync(GatewayClusterDefinition cluster, CancellationToken cancellationToken);
        Task UpdateClusterAsync(GatewayClusterDefinition cluster, CancellationToken cancellationToken);
    }
}
