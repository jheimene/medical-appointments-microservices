using MediatR;

namespace PatientService.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get;  }
    }
}
