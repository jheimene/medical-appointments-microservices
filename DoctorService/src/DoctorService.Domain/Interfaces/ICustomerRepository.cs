using DoctorService.Domain.Entities;

namespace DoctorService.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        void CreateAsync(Customer customer);
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
        Task DeleteAsync(CustomerId id, CancellationToken cancellationToken = default);
        Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default);
        Task<Customer?> GetByDocumentNumber(string documentNumber, CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);
    }
}
