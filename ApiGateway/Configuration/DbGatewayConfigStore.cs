using Microsoft.EntityFrameworkCore;
using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Contracts;
using Security.ApiGateway.Yarp.Data;
using Security.ApiGateway.Yarp.Models;

namespace Security.ApiGateway.Yarp.Configuration
{
    public sealed class DbGatewayConfigStore(YarpConfigDbContext context) : IGatewayConfigStore
    {
        private readonly YarpConfigDbContext _context = context;

        public async Task<GatewayConfigSnapshot> GetAsync(CancellationToken cancellationToken)
        {
            var routes = await _context.Routes
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.RouteId)
            .Select(x => new GatewayRouteDefinition(
                x.RouteId,
                x.ClusterId,
                x.Order,
                x.Methods,
                x.PathPattern,
                x.RemovePrefix,
                x.AuthorizationPolicy))
            .ToListAsync(cancellationToken);

            var clusters = await _context.Clusters
                .AsNoTracking()
                .Include(x => x.Destinations.Where(d => d.IsActive))
                .Where(x => x.IsActive)
                .OrderBy(x => x.ClusterId)
                .ToListAsync(cancellationToken);

            var clusterDefinitions = clusters
                .Select(cluster => new GatewayClusterDefinition(
                    cluster.ClusterId,
                    cluster.LoadBalancingPolicy,
                    cluster.HealthCheckPath,
                    cluster.Destinations
                        .OrderBy(d => d.DestinationId)
                        .Select(d => new GatewayDestinationDefinition(
                            d.DestinationId,
                            d.Address))
                        .ToList()))
                .ToList();

            return new GatewayConfigSnapshot(routes, clusterDefinitions);
        }

        public async Task SaveAsync(GatewayConfigSnapshot snapshot, CancellationToken cancellationToken)
        {
            ValidateSnapshot(snapshot);

            await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

            // Reemplazo completo del snapshot.
            // Muy práctico cuando YARP trabaja con rutas/clusters completos.
            await _context.Destinations.ExecuteDeleteAsync(cancellationToken);
            await _context.Routes.ExecuteDeleteAsync(cancellationToken);
            await _context.Clusters.ExecuteDeleteAsync(cancellationToken);

            var clusterEntities = snapshot.Clusters
                .Select(cluster => new ProxyCluster
                {
                    ClusterId = cluster.ClusterId,
                    LoadBalancingPolicy = cluster.LoadBalancingPolicy,
                    IsActive = true,
                    Destinations = cluster.Destinations
                        .Select(dest => new ProxyDestionation
                        {
                            DestinationId = dest.DestinationId,
                            Address = dest.Address,
                            IsActive = true
                        })
                        .ToList()
                })
                .ToList();

            await _context.Clusters.AddRangeAsync(clusterEntities, cancellationToken);

            var routeEntities = snapshot.Routes
                .Select(route => new ProxyRoute
                {
                    RouteId = route.RouteId,
                    ClusterId = route.ClusterId,
                    PathPattern = route.Path,
                    RemovePrefix = route.RemovePrefix,
                    Order = route.Order,
                    AuthorizationPolicy = route.AuthorizationPolicy,
                    IsActive = true
                })
                .ToList();

            await _context.Routes.AddRangeAsync(routeEntities, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }

        public async Task UpsertRouteAsync(GatewayRouteDefinition route, CancellationToken cancellationToken)
        {
            ValidateRoute(route);

            var clusterExists = await _context.Clusters
                .AnyAsync(x => x.IsActive && x.ClusterId == route.ClusterId, cancellationToken);

            if (!clusterExists)
                throw new InvalidOperationException(
                    $"No existe un cluster activo con ClusterId '{route.ClusterId}'.");

            var entity = await _context.Routes
                .FirstOrDefaultAsync(x => x.RouteId == route.RouteId, cancellationToken);

            if (entity is null)
            {
                entity = new ProxyRoute
                {
                    RouteId = route.RouteId,
                    ClusterId = route.ClusterId,
                    PathPattern = route.Path,
                    RemovePrefix = route.RemovePrefix,
                    Order = route.Order,
                    AuthorizationPolicy = route.AuthorizationPolicy,
                    IsActive = true
                };

                await _context.Routes.AddAsync(entity, cancellationToken);
            }
            else
            {
                entity.ClusterId = route.ClusterId;
                entity.PathPattern = route.Path;
                entity.RemovePrefix = route.RemovePrefix;
                entity.Order = route.Order;
                entity.AuthorizationPolicy = route.AuthorizationPolicy;
                entity.IsActive = true;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRouteAsync(string routeId, CancellationToken cancellationToken)
        {
            var entity = await _context.Routes
                .FirstOrDefaultAsync(x => x.RouteId == routeId, cancellationToken);

            if (entity is null)
                return;

            _context.Routes.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private static void ValidateSnapshot(GatewayConfigSnapshot snapshot)
        {
            var clusterIds = snapshot.Clusters
                .Select(x => x.ClusterId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var cluster in snapshot.Clusters)
            {
                if (string.IsNullOrWhiteSpace(cluster.ClusterId))
                    throw new InvalidOperationException("ClusterId es obligatorio.");

                if (cluster.Destinations.Count == 0)
                    throw new InvalidOperationException(
                        $"El cluster '{cluster.ClusterId}' debe tener al menos un destino.");
            }

            foreach (var route in snapshot.Routes)
            {
                ValidateRoute(route);

                if (!clusterIds.Contains(route.ClusterId))
                    throw new InvalidOperationException(
                        $"La ruta '{route.RouteId}' referencia un cluster inexistente: '{route.ClusterId}'.");
            }
        }

        private static void ValidateRoute(GatewayRouteDefinition route)
        {
            if (string.IsNullOrWhiteSpace(route.RouteId))
                throw new InvalidOperationException("RouteId es obligatorio.");

            if (string.IsNullOrWhiteSpace(route.ClusterId))
                throw new InvalidOperationException("ClusterId es obligatorio.");

            if (string.IsNullOrWhiteSpace(route.Path))
                throw new InvalidOperationException("Path es obligatorio.");
        }
    }
}
