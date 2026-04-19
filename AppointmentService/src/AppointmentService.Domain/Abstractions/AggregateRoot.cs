
namespace OrderService.Domain.Abstractions
{
    public abstract class AggregateRoot : BaseAuditableEntity
    {
        private readonly List<DomainEvent> _domainEvents = [];
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        protected AggregateRoot() { }

        protected AggregateRoot(Guid id) : base(id) { }


        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        protected void RemoveDomainEvent(DomainEvent domain) => _domainEvents.Remove(domain);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
