namespace OrderService.Contracts.Dtos
{
    public sealed record ZoneRequest(
        Guid ZoneId,
        int Quantity,
        decimal UnitPrice
    );
}
