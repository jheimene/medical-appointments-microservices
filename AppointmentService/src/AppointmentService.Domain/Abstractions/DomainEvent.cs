
namespace OrderService.Domain.Abstractions
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.Now;
    }
}
