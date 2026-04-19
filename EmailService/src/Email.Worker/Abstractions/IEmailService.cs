
using Email.Worker.Models;

namespace Email.Worker.Abstractions
{
    public interface IEmailService
    {
        Task ProcessEmailAsync(UserCreatedEvent request, CancellationToken cancellationToken = default);
    }
}
