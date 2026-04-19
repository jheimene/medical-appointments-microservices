using BffService.DTOs;

namespace BffService.Interfaces
{
    public interface IOrderSummaryComposer
    {
        Task<OrderSummaryResponse?> ComposeAsync(Guid orderId, CancellationToken cancellationToken);
    }
}
