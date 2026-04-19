namespace BffService.DTOs
{
    public sealed record ShipmentDto(
        Guid Id,
        string Status,
        string? TrackingCode
    );
}
