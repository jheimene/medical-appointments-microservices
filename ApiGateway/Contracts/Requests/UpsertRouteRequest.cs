namespace Security.ApiGateway.Yarp.Contracts.Requests
{
    public sealed record UpsertRouteRequest(
        string RouteId,
        string ClusterId,
        int OrderIndex,
        string[] Methods,
        string PathPattern
    );
}
