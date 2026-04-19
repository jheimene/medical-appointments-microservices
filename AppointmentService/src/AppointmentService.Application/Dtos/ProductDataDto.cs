
namespace OrderService.Application.Dtos
{
    public sealed record ProductDataDto(
        Guid ProductId,
        string Name,
        string Sku,
        string Currency,
        decimal Price,
        bool IsActive
    );
}
