namespace BffService.DTOs
{
    public sealed record ShipmentSummary(
        Guid ShipmentId,
        string Status,
        string? TrackingCode
    );
}
