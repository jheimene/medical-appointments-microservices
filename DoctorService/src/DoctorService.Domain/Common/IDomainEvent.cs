using MediatR;

namespace ProductService.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get;  }
    }
}
