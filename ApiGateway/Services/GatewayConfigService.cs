using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Contracts;
using Security.ApiGateway.Yarp.Contracts.Responses;

namespace Security.ApiGateway.Yarp.Services
{
    public sealed class GatewayConfigService(
        IGatewayConfigStore store,
        IGatewayRuntimeApplier runtimeApplier
        ) : IGatewayConfigService
    {

        private readonly IGatewayConfigStore _store = store;
        private readonly IGatewayRuntimeApplier _runtimeApplier = runtimeApplier;

        public async Task CreateClusterAsync(GatewayClusterDefinition cluster, CancellationToken cancellationToken)
        {
            var snapshot = await _store.GetAsync(cancellationToken);
            var clusters = snapshot.Clusters.ToList();

            // Validacion de existencia previa
            clusters.Add(cluster);

            var newSnapshot = snapshot with { Clusters = clusters };
            await _store.SaveAsync(newSnapshot, cancellationToken);
            await _runtimeApplier.ApplyAsync(newSnapshot, cancellationToken);
        }
        public async Task UpdateClusterAsync(GatewayClusterDefinition cluster, CancellationToken cancellationToken)
        {
            var snapshot = await _store.GetAsync(cancellationToken);
            var clusters = snapshot.Clusters.ToList();
            var index = clusters.FindIndex(x => x.ClusterId.Equals(cluster.ClusterId, StringComparison.OrdinalIgnoreCase));

            if (index < 0)
                throw new InvalidOperationException($"No se encontró el cluster con ID '{cluster.ClusterId}' para actualizar.");

            clusters[index] = cluster;
            var newSnapshot = snapshot with { Clusters = clusters };
            await _store.SaveAsync(newSnapshot, cancellationToken);
            await _runtimeApplier.ApplyAsync(newSnapshot, cancellationToken);
        }

        public async Task CreateRouteAsync(GatewayRouteDefinition route, CancellationToken cancellationToken)
        {
            var snapshot = await _store.GetAsync(cancellationToken);
            var routes = snapshot.Routes.ToList();

            routes.Add(route);
            var newSnapshot = snapshot with { Routes = routes };
            await _store.SaveAsync(newSnapshot, cancellationToken);
            await _runtimeApplier.ApplyAsync(newSnapshot, cancellationToken);
        }

        public async Task UpdateRouteAsync(GatewayRouteDefinition route, CancellationToken cancellationToken)
        {
            var snapshot = await _store.GetAsync(cancellationToken);
            var routes = snapshot.Routes.ToList();
            var index = routes.FindIndex(x => x.RouteId.Equals(route.RouteId, StringComparison.OrdinalIgnoreCase));

            if (index < 0)
                throw new InvalidOperationException($"No se encontró la ruta con ID '{route.RouteId}' para actualizar.");
            
            routes[index] = route;
            var newSnapshot = snapshot with { Routes = routes };
            await _store.SaveAsync(newSnapshot, cancellationToken);
            await _runtimeApplier.ApplyAsync(newSnapshot, cancellationToken);
        }

        public async Task<RouteResponse> GetRoute(string routeId, CancellationToken cancellationToken)
        {
            var snapshot = await _store.GetAsync(cancellationToken);
            var route = snapshot.Routes.FirstOrDefault(x => x.RouteId.Equals(routeId, StringComparison.OrdinalIgnoreCase));

            if (route is null)
                throw new InvalidOperationException($"No se encontró la ruta con ID '{routeId}'.");

            return new RouteResponse(
                RouteId: route.RouteId,
                ClusterId: route.ClusterId,
                OrderIndex: route.Order ?? 0,
                Methods: [.. route.Methods.Select(m => m.ToString())],
                PathPattern: route.Path
            );
        }

        public async Task<IReadOnlyList<RouteResponse>> GetRoutes(CancellationToken cancellationToken)
        {
            var snapshot = await _store.GetAsync(cancellationToken);
            var routes = snapshot.Routes;

            return [.. routes.Select(route => new RouteResponse(
                RouteId: route.RouteId,
                ClusterId: route.ClusterId,
                OrderIndex: route.Order ?? 0,
                Methods: [.. route.Methods.Select(m => m.ToString())],
                PathPattern: route.Path
            ))];
        }

        public async Task<ClusterResponse> GetCluster(string clusterId, CancellationToken cancellationToken)
        {
            var snapshot = await _store.GetAsync(cancellationToken);
            var cluster = snapshot.Clusters.FirstOrDefault(x => x.ClusterId.Equals(clusterId, StringComparison.OrdinalIgnoreCase));

            if (cluster is null)
                throw new InvalidOperationException($"No se encontró el cluster con ID '{clusterId}'.");

            return new ClusterResponse(
                ClusterId: cluster.ClusterId,
                LoadBalancingPolicy: cluster.LoadBalancingPolicy,
                HealthCheckPath: cluster.HealthCheckPath,
                Destinations: cluster.Destinations.ToDictionary(d => d.DestinationId, d => d.Address)
            );
        }

        public async Task<IReadOnlyList<ClusterResponse>> GetClusters(CancellationToken cancellationToken)
        {
            var snapshot = await _store.GetAsync(cancellationToken);
            var cluster = snapshot.Clusters;

             return [.. cluster.Select(cluster => new ClusterResponse(
                ClusterId: cluster.ClusterId,
                LoadBalancingPolicy: cluster.LoadBalancingPolicy,
                HealthCheckPath: cluster.HealthCheckPath,
                Destinations: cluster.Destinations.ToDictionary(d => d.DestinationId, d => d.Address)
            ))];
        }
    }
}
