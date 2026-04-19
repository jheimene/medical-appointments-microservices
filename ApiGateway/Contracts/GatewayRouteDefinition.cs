
using Security.ApiGateway.Yarp.Models;

namespace Security.ApiGateway.Yarp.Contracts
{
    public sealed record GatewayRouteDefinition(
        string RouteId,
        string ClusterId,
        int? Order,
        ICollection<Method> Methods,
        string Path,
        string? RemovePrefix,
        string? AuthorizationPolicy
    );
}
