using MediatR;

namespace DispatchService.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get;  }
    }
}
