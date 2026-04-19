namespace OrderService.Application.Dtos
{
    public sealed record OrderDetailDto(
        Guid OrderId,
        string OrderNumber,
        string Status,
        string Currency,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal TaxTotal,
        decimal Total
    );
}
