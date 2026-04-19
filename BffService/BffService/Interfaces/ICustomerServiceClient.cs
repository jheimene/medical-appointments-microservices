using BffService.DTOs;

namespace BffService.Interfaces
{
    public interface ICustomerServiceClient
    {
        Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
