using OrderService.Application.Dtos;

namespace OrderService.Application.Abstractions.Services
{
    public interface ICustomerReadService
    {
        Task<CustomerDataDto?> GetByIdAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
