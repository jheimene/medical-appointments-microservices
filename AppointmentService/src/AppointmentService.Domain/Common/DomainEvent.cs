
namespace AppointmentService.Domain.Common
{
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.Now;
    }
}
