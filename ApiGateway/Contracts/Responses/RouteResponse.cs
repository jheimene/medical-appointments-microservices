namespace Security.ApiGateway.Yarp.Contracts.Responses
{
    public sealed record RouteResponse(
        string RouteId,
        string ClusterId,
        int OrderIndex,
        string[] Methods,
        string PathPattern
     );
}
