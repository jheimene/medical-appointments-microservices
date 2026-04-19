namespace BuildingBlocks.Api.ErrorHandling
{
    public sealed record ErrorDetail(
        string Code,
        string Message,
        string Type,
        Dictionary<string, object>? Metadata,
        string TraceId
        );    
}