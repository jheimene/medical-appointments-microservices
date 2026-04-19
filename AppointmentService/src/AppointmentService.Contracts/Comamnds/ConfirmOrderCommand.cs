
using OrderService.Contracts.Dtos;

namespace OrderService.Contracts.Comamnds
{
    public sealed record ConfirmOrderCommand(
        Guid OrderId,
        Guid EventId
    );
}
