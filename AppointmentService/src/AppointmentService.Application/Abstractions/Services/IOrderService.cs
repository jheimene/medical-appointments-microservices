namespace OrderService.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(Guid customerId, List<(Guid productId, int quantity)> items);
        Task ConfirmOrderAsync(Guid orderId);
        Task CancelOrderAsync(Guid orderId);
    }
}
