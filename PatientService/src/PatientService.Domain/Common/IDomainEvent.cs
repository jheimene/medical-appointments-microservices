using MediatR;

namespace CustomerService.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get;  }
    }
}
