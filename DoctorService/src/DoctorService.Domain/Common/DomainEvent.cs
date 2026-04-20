
namespace DoctorService.Domain.Common
{
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.Now;
    }
}
